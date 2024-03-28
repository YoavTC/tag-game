' Git Commit Count.vbs
' VBScript to display the number of commits in the current Git repository

Dim objShell, objFSO, strScriptPath, strRepoPath, strCommand, strOutput

' Get the directory of the script file
strScriptPath = WScript.ScriptFullName
Set objFSO = CreateObject("Scripting.FileSystemObject")
strRepoPath = objFSO.GetParentFolderName(strScriptPath)

' Create a shell object to execute commands
Set objShell = CreateObject("WScript.Shell")

' Change directory to the Git repository
objShell.CurrentDirectory = strRepoPath

' Command to get the number of commits
strCommand = "git rev-list --count HEAD"

' Execute the command and capture the output
strOutput = ExecuteCommand(strCommand)

' Display the number of commits
WScript.Echo "Number of commits in the repository: " & strOutput

' Function to execute a command and capture the output
Function ExecuteCommand(command)
    Dim objExec, strCmdOutput
    Set objExec = objShell.Exec(command)
    strCmdOutput = objExec.StdOut.ReadAll
    ExecuteCommand = Trim(strCmdOutput)
End Function
