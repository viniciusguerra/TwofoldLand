using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class WrongCommandSyntaxException : Exception
{
    public WrongCommandSyntaxException() { }
    public WrongCommandSyntaxException(string message) : base(message) { }
    public WrongCommandSyntaxException(string message, Exception inner) : base(message, inner) { }
    protected WrongCommandSyntaxException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }
}

[Serializable]
public class Command
{
    public Type abstractionInterfaceType;
    public MethodInfo methodInfo;
    public object[] parameters;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("{0}.{1}(", abstractionInterfaceType.Name, methodInfo.Name);

        ParameterInfo[] p = methodInfo.GetParameters();

        if(p.Length > 0)
        {
            for (int i = 0; i < p.Length - 1; i++)
			{
                sb.Append(p[i].ParameterType.ToString() + ",");
			}

            sb.Append(p[p.Length - 1]);
        }

        sb.Append(")");

        return sb.ToString();
    }

    public Command()
    {

    }

    public Command(Type abstractionInterfaceType, MethodInfo methodInfo, object[] parameters)
    {        
        this.abstractionInterfaceType = abstractionInterfaceType;
        this.methodInfo = methodInfo;
        this.parameters = parameters;
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public static Type FindInterface(string interfaceName)
    {
        Type[] assemblyTypes = typeof(IAbstraction).Assembly.GetTypes();
        
        foreach (Type t in assemblyTypes)
        {
            if (t.IsAssignableFrom(typeof(IAbstraction)))
            {
                if (t.Name.Equals(interfaceName))
                {
                    return t;
                }
            }
        }

        throw new MissingMemberException();
    }

    ///<exception cref="MissingMethodException">Thrown when there is no method with the given name</exception>
    public static MethodInfo FindMethodInInterface(Type interfaceType, string methodName)
    {
        foreach (MethodInfo m in interfaceType.GetMethods())
        {
            if (m.Name.Equals(methodName))
            {
                return m;
            }
        }

        throw new MissingMethodException();
    }

    ///<exception cref="MissingMethodException">Thrown when there is no method with the given name</exception>
    public static MethodInfo FindMethodInInterface(Type interfaceType, string methodName, int parameterCount)
    {
        foreach (MethodInfo m in interfaceType.GetMethods())
        {
            if (m.Name.Equals(methodName))
            {
                ParameterInfo[] parameters = m.GetParameters();

                //if the parameter count in this method match the sent parameter count, build the Command
                if (parameters.Length == parameterCount)
                {
                    return m;
                }
            }
        }

        throw new MissingMethodException();
    }

    ///<exception cref="WrongCommandSyntaxException">Thrown when the given string doesn't meet the correct syntax</exception>
    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    ///<exception cref="MissingMethodException">Thrown when there is no method with the given name</exception>
    public static Command BuildCommand(string commandString)
    {
        if (Regex.IsMatch(commandString, GlobalDefinitions.TerminalCommandRegexPattern))
        {            
            List<string> commandSplit = new List<string>(commandString.Split(GlobalDefinitions.CommandSplitCharacters, System.StringSplitOptions.RemoveEmptyEntries));

            Type[] assemblyTypes = typeof(IAbstraction).Assembly.GetTypes();

            //goes through every interface implementing IAbstraction and finds the matching type
            foreach(Type t in assemblyTypes)
            {
                if (typeof(IAbstraction).IsAssignableFrom(t))
                {
                    if (t.Name.Equals(commandSplit[0]))
                    {
                        //goes through every Method in the found Type and finds the one matching the Command
                        foreach (MethodInfo m in t.GetMethods())
                        {
                            if (m.Name.Equals(commandSplit[1]))
                            {
                                ParameterInfo[] parameters = m.GetParameters();

                                //if the parameter count in this method match the sent parameter count, build the Command
                                if (parameters.Length == commandSplit.Count - 2)
                                {
                                    try
                                    {
                                        return new Command(t, m, commandSplit.GetRange(2, commandSplit.Count - 2).ToArray());
                                    }
                                    #pragma warning disable 0168
                                    catch(Exception e)
                                    #pragma warning restore 0168
                                    {
                                        return new Command(t, m, null);
                                    }
                                }
                            }
                        }

                        //if no method with the given name and parameters is found
                        throw new MissingMethodException();
                    }
                }
            }

            //if no interface implementing IAbstraction with the given name is found
            throw new MissingMemberException();
        }
        else
            //if the Command's syntax doesn't match the Regex pattern
            throw new WrongCommandSyntaxException();
    }
}
