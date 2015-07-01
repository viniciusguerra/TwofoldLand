using UnityEngine;
using System.Collections;

public static class GlobalDefinitions
{
    //Misc Strings
    public const string ActorTag = "Actor";   
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

    //Regex Patterns
    //Tested at http://www.regexr.com/
    public const string TerminalCommandRegexPattern = @"^(?:\w+)(?:\.)(?:\w+)(?:\()(?:\w|\,\w){0,}(?:\))$";
    public const string TerminalInterfaceRegexPattern = @"^(?:\w+)$";
    public const string TerminalInterfaceAndMethodRegexPattern = @"^(?:\w+)\.(?:|\w+)$";
}

//TODO FEATURE Write Spells(collections of commands). Spells cost Resource and are compiled at a Compiler. Send them via hotkey after assigning them.
//TODO FEATURE Cast assigned Spells via Spellcasting Commorose. By activating it, time slows down like when choosing Signs on The Witcher 3.