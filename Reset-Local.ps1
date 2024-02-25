# You can use this script to reset the app, i.e., it'll run the setup on the next launch again. Execute it only when the
# app is not running.

Remove-Item '.\Lombiq.TrainingDemo.Web\App_Data' -Recurse && Write-Output 'Successfully reset website.'
