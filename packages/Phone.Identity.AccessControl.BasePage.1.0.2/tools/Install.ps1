param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]
$readmeFile = $path::Combine($path::GetDirectoryName($project.FileName), "App_Readme\Phone.Identity.Controls.BasePage.Readme.htm")
$DTE.ItemOperations.Navigate($readmeFile, [EnvDTE.vsNavigateOptions]::vsNavigateOptionsNewWindow)
