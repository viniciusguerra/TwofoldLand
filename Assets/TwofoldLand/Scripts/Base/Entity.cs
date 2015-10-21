using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    private Type[] implementedInterfaces;
    private List<Type> knownInterfaceList;

    public List<Type> KnownInterfaceList
    {
        get
        {
            UpdateKnownInterfaceList();

            return knownInterfaceList;
        }
    }

    private void UpdateKnownInterfaceList()
    {
        if (knownInterfaceList == null)
            knownInterfaceList = new List<Type>();

        if (implementedInterfaces == null)
            implementedInterfaces = GetType().GetInterfaces();

        knownInterfaceList.Clear();

        for (int i = 0; i < implementedInterfaces.Length; i++)
        {
            if (Player.Instance.KnowsInterface(implementedInterfaces[i]))
            {
                knownInterfaceList.Add(implementedInterfaces[i]);
            }
        }
    }

    ///<exception cref="MethodAccessException">Thrown when the method being called is not accessible</exception>
    ///<exception cref="NotImplementedException">Thrown when there is no implemented interface with the given name</exception>    
    ///<exception cref="MissingMethodException">Thrown when there is no method with the given name</exception>
    ///<exception cref="TargetParameterCountException">Thrown when the target method has no overload with the given parameter count</exception>    
    public void SubmitCommand(Command command)
    {
        //Short invoking
        command.methodInfo.Invoke(this, command.parameters);

        //Long invoking
        ////Checks if the current Type implements the given Type
        ////Will proceed to call the given Method if it is implemented from the given interface
        //if (command.interfaceType.IsAssignableFrom(GetType()))
        //{
        //    MethodInfo[] implementedMethods = command.interfaceType.GetMethods();

        //    foreach (MethodInfo m in implementedMethods)
        //    {
        //        //Checks if this Command's Method name is found within the interface
        //        //If it is, checks parameters for correct override call
        //        if (m.Equals(command.methodInfo))
        //        {
        //            if (m.IsPublic)
        //            {
        //                try
        //                {
        //                    m.Invoke(this, command.parameters);
        //                }
        //                catch (TargetParameterCountException t)
        //                {
        //                    throw t;
        //                }

        //                return;
        //            }
        //            else
        //                throw new MethodAccessException();
        //        }
        //    }

        //    throw new MissingMethodException();
        //}
        //else
        //    throw new NotImplementedException();
    }

    public void SubmitSpell(Spell spell)
    {
        string errorMessage = string.Empty;

        foreach (Command c in spell.commands)
        {
            try
            {
                SubmitCommand(c);
            }
#pragma warning disable 0168
            catch (MethodAccessException mae)
            {
                errorMessage = GlobalDefinitions.InvalidMethodErrorMessage;
                HUD.Instance.log.ShowMessage(errorMessage + " - Spell interrupted");
                return;
            }
            catch (NotImplementedException nie)
            {
                errorMessage = GlobalDefinitions.InvalidMethodErrorMessage;
                HUD.Instance.log.ShowMessage(errorMessage + " - Spell interrupted");
                return;
            }
            catch (MissingMethodException mme)
            {
                errorMessage = GlobalDefinitions.InvalidMethodErrorMessage;
                HUD.Instance.log.ShowMessage(errorMessage + " - Spell interrupted");
                return;
            }
            catch (TargetParameterCountException tpce)
#pragma warning restore 0168
            {
                errorMessage = GlobalDefinitions.InvalidParametersErrorMessage;
                HUD.Instance.log.ShowMessage(errorMessage + " - Spell interrupted");
                return;
            }
        }
    }
}
