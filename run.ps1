$branch = "js/v2/full"
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
git checkout -b workshop-@@nexttag@@ @@nexttag@@
git merge @@nexttag@@-test1
git checkout --ours .
git add .
git commit -m "Merge with test branch"
"@

function generateTagsForStep($stepNum){
	([array](1..$testsNbByStep.$stepNum | %{ "step" + $stepNum + "-test" + $_ })) + ("step" + $stepNum)
}

function generateTags(){
	1..$testsNbByStep.Count | %{ generateTagsForStep $_ }
}

function clean(){
	git reset --hard HEAD
	#git clean -dfx
	git checkout master
	git branch -D $testBranch
	git branch -D $solutionBranch
	git branch -D $workshopBranch
	1..$testsNbByStep.Count | %{ git branch -D ("workshop-step" + $_) }
	generateTags | %{ git tag -d $_ }
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

function addNavigationCommand(){
	$hasNextStep = hasNextStep
	$hasNextTestForCurrentStep = hasNextTestForCurrentStep
	if((hasNextStep) -or (hasNextTestForCurrentStep)) {
		$nextCommandTemplate.Replace("@@nexttag@@", (getNextTestTag)) | out-file 'next.bat' -enc ascii
		git add next.bat > $null

		git commit -m "Add test navigation commands" > $null
	}
}

function pickCommitForSolution($line){
	$parts = $line.split(' ')
	$hash = $parts[0]

	Write-Host ("cherry-pick " + $line)

	$isKoTest = $line -like '* KO]*'
	$isFirstTestOfStep = $currentTestOfStep -eq 1
	if($isKoTest -and $isFirstTestOfStep -and (hasNextStep)){
		$jumpToNextStepCommandTemplate.Replace("@@nexttag@@", (getNextStepTag)) | out-file 'jumpToNextStep.bat' -enc ascii
		git add jumpToNextStep.bat > $null

		git commit -m "Add step navigation commands" > $null
	}

	git cherry-pick $hash > $null

	if($isKoTest -and $isFirstTestOfStep){
		Write-Host ("add tag " + (getCurrentStepTag))

		git tag (getCurrentStepTag) > $null
	}

	if($isKoTest){
		nextTest
	}
}

function pickCommitForTest($line){
	$parts = $line.split(' ')
	$hash = $parts[0]

	Write-Host ("cherry-pick " + $line)

	git cherry-pick $hash > $null

	if ($line -like '* KO]*') { 
		Write-Host ("add tag " + (getCurrentTestTag))

		addNavigationCommand

		git tag (getCurrentTestTag) > $null
		nextTest
	}
}

clean

sleep 5

$currentStep = 1
$currentTestOfStep = 1
git checkout -b $solutionBranch origin/master
git log $branch --pretty=tformat:'%h %s' --reverse -E HEAD.. | %{ pickCommitForSolution $_ }

sleep 2

$currentStep = 1
$currentTestOfStep = 1
git checkout -b $testBranch origin/master
git log $solutionBranch --pretty=tformat:'%h %s' --grep '[^K]\][^\[]' --grep '^[^\[]' --reverse -E HEAD.. | %{ pickCommitForTest $_ }

git checkout -b $workshopBranch "step1-test1"