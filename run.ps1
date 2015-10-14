$languages = @{ "js"="origin/js/v2/full" }
$testsNbByStep = @{ 1=4; 2=1; 3=4; 4=4; 5=2 }

$testBranch = "test"
$solutionBranch = "solution"
$workshopBranch = "workshop"

$nextCommandTemplate = @"
@echo off

git add -A 
git commit -m ""Resolve test"" 
git merge @@nexttag@@
"@

$jumpToNextStepCommandTemplate = @"
@echo off

git add -A 
git commit -m ""Abort test"" 
git checkout -b $workshopBranch-@@nexttag@@ @@nexttag@@
git merge @@nexttag@@-test1
git checkout --ours .
git add .
git commit -m "Merge with test branch"
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

function addStepNavigationCommand($nextStepTag){
	$jumpToNextStepCommandTemplate.Replace("@@nexttag@@", $nextStepTag) | out-file 'jumpToNextStep.bat' -enc ascii
	git add jumpToNextStep.bat > $null

	git commit -m "Add step navigation commands" > $null
}

function pickCommitForSolution($line){
	$hash = extraCommitHashOfLog $line

	Write-Host -NoNewline "."

	$isKoTest = isFailedTestCommit $line
	$isFirstTestOfStep = $currentTestOfStep -eq 1
	if($isKoTest -and $isFirstTestOfStep -and (hasNextStep)){
		addStepNavigationCommand (getNextStepTag)
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

function initializeSolutionBranch($referenceBranch){
	Write-Host "Initialize solution branch"

	resetTestCounter

	git checkout -b $solutionBranch origin/master > $null
	getCommitLog $referenceBranch | %{ pickCommitForSolution $_ }

	Write-Host "Done"
}

function addNavigationCommand($nextTestTag){
	$nextCommandTemplate.Replace("@@nexttag@@", $nextTestTag) | out-file 'next.bat' -enc ascii
	git add next.bat > $null

	git commit -m "Add test navigation commands" > $null
}

function pickCommitForTest($line){
	$hash = extraCommitHashOfLog $line
	
	Write-Host -NoNewline "."

	git cherry-pick $hash > $null

	if (isFailedTestCommit $line) {
		if((hasNextStep) -or (hasNextTestForCurrentStep)) {
			addNavigationCommand (getNextTestTag)
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

	git checkout -b $testBranch origin/master > $null
	getCommitLogWithoutGreenTest $solutionBranch | %{ pickCommitForTest $_ }

	Write-Host "Done"
}

function InitializeWorkflow(){
	Write-Host "Initialize workspace"
	
	resetTestCounter

	git checkout -b $workshopBranch (getCurrentTestTag)
}

$selectedLanguage = askLanguage

clean
initializeSolutionBranch $languages.$selectedLanguage
initializeTestBranch
InitializeWorkflow

Write-Host "Koan OK"
Write-Host ""
Write-Host ""

Get-Content welcome.txt | Write-Host -f green