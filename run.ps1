$languages = @{ "js"="origin/js/v2"; "csharp"="origin/csharp/v2.3";"csharp-core"="origin/csharp-core/v2"; "java"="origin/java/v2.1"; "scala"="origin/scala/v2.1"; "php"="origin/php/v2.1"; "fsharp"="origin/fsharp/v2" }
$testsNbByStep = @{ 1=4; 2=1; 3=4; 4=4; 5=2 }

$masterBranch = "origin/master"
$testBranch = "test"
$solutionBranch = "solution"
$workshopBranch = "workshop"

$batWrapperTemplate = @"
@echo off

PowerShell -ExecutionPolicy Bypass -File @@scriptname@@.ps1
"@

$nextCommandTemplate = @"
git add -A 
git commit -m "Resolve test"
git merge @@nexttag@@
"@

$jumpToNextStepCommandTemplate = @"
git add -A
git commit -m "Abort test"
git checkout -b $workshopBranch-@@nexttag@@ @@nexttag@@ 
git merge @@nexttag@@-test1 *> `$null`
git checkout --ours . 
git add . 
git commit -m "Merge with test branch" 
"@

$displayNextStepMessageTemplate = @"
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step@@stepNum@@.txt | Write-Host -f green
Write-Host ""
Write-Host ""
"@

$displayNextTestMessageTemplate = @"
Write-Host ""
Write-Host ""
Write-Host -f green "========================"
Write-Host -f green "===  STEP @@stepNum@@ - Test @@testNum@@ ==="
Write-Host -f green "========================"
Write-Host ""
Write-Host ""
"@

$displayEndMessageTemplate = @"
Write-Host ""
Write-Host ""
Get-Content stepsDoc/end.txt | Write-Host -f green
Write-Host ""
Write-Host ""
"@

function AskParametreWithValues($name, $values){
    do {
        $value = Read-Host ($name + " (" + ($values -join ", ") + ")")
    } while(-Not ($values -contains $value))

    return $value
}

function askLanguage(){
	AskParametreWithValues "Language" $languages.Keys
}

function generateTagsForStep($stepNum){
	[array]$testTags = 1..$testsNbByStep.$stepNum | %{ "step" + $stepNum + "-test" + $_ }
	[array]$stepTags = ("step" + $stepNum)
	
	$testTags + $stepTags
}

function generateTags(){
	1..$testsNbByStep.Count | %{ generateTagsForStep $_ }
}

function getCurrentTestTag(){
	"step" + $currentStep + "-test" + $currentTestOfStep
}

function getNextTestTag(){
	if($testsNbByStep.$currentStep -le $currentTestOfStep){
		"step" + ($currentStep + 1) + "-test1"
	} else {
		"step" + $currentStep + "-test" + ($currentTestOfStep + 1)
	}
}

function getNextTestNum(){
	if($testsNbByStep.$currentStep -le $currentTestOfStep){
		return 1
	} else {
		return ($currentTestOfStep + 1)
	}
}

function getCurrentStepTag(){
	"step" + $currentStep
}

function getNextStepTag(){
	"step" + ($currentStep + 1)
}

function resetTestCounter(){
	$script:currentStep = 1
	$script:currentTestOfStep = 1
}

function nextTest(){
	if($testsNbByStep.$currentStep -le $currentTestOfStep){
		$script:currentStep++
		$script:currentTestOfStep = 1
	} else {
		$script:currentTestOfStep++
	}
}

function hasNextTestForCurrentStep(){
	$currentTestOfStep -lt $testsNbByStep.$currentStep
}

function hasNextStep(){
	$currentStep -lt $testsNbByStep.Count
}

function clean(){
	Write-Host "Clean repository..."

	git clean -d -x -f > $null
	git reset --hard HEAD > $null
	git checkout master > $null
	git branch -D $testBranch *> $null
	git branch -D $solutionBranch *> $null
	git branch -D $workshopBranch *> $null
	generateTags | %{ git branch -D ($workshopBranch + "-" + $_) } *> $null
	generateTags | %{ git tag -d $_ } *> $null
}

function extraCommitHashOfLog($line){
	$parts = $line.split(' ')
	$parts[0]
}

function isFailedTestCommit($line){
	$line -like '* KO]*'
}

function addStepNavigationCommand($nextStepTag, $nextStepNum){
	$nextCommandContent = $jumpToNextStepCommandTemplate.Replace("@@nexttag@@", $nextStepTag) + "`r`n" + $displayNextStepMessageTemplate.Replace("@@stepNum@@", $nextStepNum)
	
	$nextCommandContent | out-file 'jumpToNextStep.ps1' -enc ascii
	git add jumpToNextStep.ps1 > $null

	git commit -m "Add step navigation commands" > $null
}

function addEndStepNavigationCommand(){
	$displayEndMessageTemplate | out-file 'jumpToNextStep.ps1' -enc ascii
	git add jumpToNextStep.ps1 > $null

	git commit -m "Add end step navigation commands" > $null
}

function pickCommitForSolution($line){
	$hash = extraCommitHashOfLog $line

	Write-Host -NoNewline "."

	$isKoTest = isFailedTestCommit $line
	$isFirstTestOfStep = $currentTestOfStep -eq 1
	if($isKoTest -and $isFirstTestOfStep){
		if((hasNextStep)) {
			addStepNavigationCommand (getNextStepTag) ($currentStep + 1)
		} else {
			addEndStepNavigationCommand
		}
	}

	git cherry-pick $hash > $null

	if($isKoTest -and $isFirstTestOfStep){
		git tag (getCurrentStepTag) > $null

		Write-Host ((getCurrentStepTag) + " Ok")
	}

	if($isKoTest){
		nextTest
	}
}

function getCommitLog($branch){
	git log $branch --pretty=tformat:'%h %s' --reverse -E HEAD..
}

function initializeNavigationScript(){
	$batWrapperTemplate.Replace("@@scriptname@@", 'jumpToNextStep') | out-file 'jumpToNextStep.bat' -enc ascii
	$batWrapperTemplate.Replace("@@scriptname@@", 'next') | out-file 'next.bat' -enc ascii
	
	git add jumpToNextStep.bat > $null
	git add next.bat > $null
	
	git commit -m "Add bat wrapper to navigation commands" > $null
}

function initializeSolutionBranch($referenceBranch){
	Write-Host "Initialize solution branch"

	resetTestCounter

	git checkout -b $solutionBranch $masterBranch > $null
	initializeNavigationScript
	getCommitLog $referenceBranch | %{ pickCommitForSolution $_ }

	Write-Host "Done"
}

function addNavigationCommand($nextTestTag, $nextTestNum, $currentStepNum){
	$nextCommandContent = $nextCommandTemplate.Replace("@@nexttag@@", $nextTestTag)
	if((getNextTestNum) -eq 1) {
		$nextCommandContent += "`r`n" + $displayNextStepMessageTemplate.Replace("@@stepNum@@", $currentStepNum + 1)
	} else {
		$nextCommandContent += "`r`n" + $displayNextTestMessageTemplate.Replace("@@stepNum@@", $currentStepNum).Replace("@@testNum@@", $nextTestNum)
	}

	$nextCommandContent | out-file 'next.ps1' -enc ascii
	git add next.ps1 > $null

	git commit -m "Add test navigation commands" > $null
}

function addEndNavigationCommand(){
	$displayEndMessageTemplate | out-file 'next.ps1' -enc ascii
	git add next.ps1 > $null

	git commit -m "Add end test navigation commands" > $null
}

function pickCommitForTest($line){
	$hash = extraCommitHashOfLog $line
	
	Write-Host -NoNewline "."

	git cherry-pick $hash > $null

	if (isFailedTestCommit $line) {
		if((hasNextStep) -or (hasNextTestForCurrentStep)) {
			addNavigationCommand (getNextTestTag) (getNextTestNum) ($currentStep)
		} else {
			addEndNavigationCommand
		}

		git tag (getCurrentTestTag) > $null
		Write-Host ((getCurrentTestTag) + " Ok")

		nextTest
	}
}

function getCommitLogWithoutGreenTest($branch){
	git log $branch --pretty=tformat:'%h %s' --grep '[^K]\][^\[]' --grep '^[^\[]' --reverse -E HEAD..
}

function initializeTestBranch(){
	Write-Host "Initialize test branch"

	resetTestCounter

	git checkout -b $testBranch $masterBranch > $null
	getCommitLogWithoutGreenTest $solutionBranch | %{ pickCommitForTest $_ }

	Write-Host "Done"
}

function initializeWorkflow(){
	Write-Host "Initialize workspace"
	
	resetTestCounter

	git checkout -b $workshopBranch (getCurrentTestTag)
}

function customInitialize(){
	if (Test-Path ./initialize.bat) {
		& ./initialize.bat
	}
}

function displayWarningIfNotMaster(){
	$currentBranch = git name-rev --name-only HEAD
	if($currentBranch -ne "master") {
		$confirmation = Read-Host "Are you sure you want to reinit everything? All your work will be lost! (y/N)"
		if ($confirmation.ToLower() -ne 'y') {
			exit  
		}
	}
}

displayWarningIfNotMaster

$selectedLanguage = askLanguage

clean
initializeSolutionBranch $languages.$selectedLanguage
initializeTestBranch
initializeWorkflow

customInitialize

Write-Host "Koan OK"
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step1.txt | Write-Host -f green
Write-Host ""
Write-Host ""
