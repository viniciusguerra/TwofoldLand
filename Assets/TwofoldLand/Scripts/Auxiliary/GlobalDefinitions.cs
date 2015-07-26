using UnityEngine;
using System.Collections;

public static class GlobalDefinitions
{
    //Values
    public const int AuraCostConstant = 64;

    //Tags
    public const string PlayerTag = "Player";
    public const string ActorTag = "Actor";
    public const string PathTag = "Path";
    public const string AuraTag = "Aura";
    public const string CompilerTag = "Compiler";

    //Misc Strings     
    public static readonly char[] CommandSplitCharacters = {'.','(',',',')'};
    public const string SelectedMaterialPath = "Materials/SelectedMaterial";
    public const string SpellSuffix = ".spl";
    public const string FeedbackUIFontPath = "Fonts/consola";
    
    //Error Messages
    public const string WrongSyntaxErrorMessage = "Wrong Syntax";
    public const string UnknownInterfaceErrorMessage = "Invalid Interface";
    public const string InvalidInterfaceErrorMessage = "Invalid Interface";
    public const string InvalidMethodErrorMessage = "Invalid Method";
    public const string UnnaccessibleMethodErrorMessage = "Unnaccessible Method";
    public const string NoTargetErrorMessage = "No Selected Target";
    public const string InvalidParametersErrorMessage = "Invalid Parameters";
    public const string InvalidAddressErrorMessage = "Invalid Use of Memory Address";

    //Buttons
    public const string CodexButton = "Codex";
    public const string StorageButton = "Storage";
    public const string NotesButton = "Notes";

    //Regex Patterns
    //Tested at http://www.regexr.com/
    public const string TerminalCommandRegexPattern = @"^(?:\w+)(?:\.)(?:\w+)(?:\()(?:\w|\,\w){0,}(?:\))$";
    public const string TerminalInterfaceRegexPattern = @"^(?:\w+)$";
    public const string TerminalInterfaceAndMethodRegexPattern = @"^(?:\w+)\.(?:|\w+)$";
    public const string StorageAddressPattern = @"^0x[A-F0-9]+$";
}