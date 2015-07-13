using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using System;

public class Actor : MonoBehaviour, IPointerClickHandler
{    
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private Type[] implementedInterfaces;
    private List<string> interfaceList;

    public void DisplayInterfaces()
    {
        bool showHealthBar = false;

        interfaceList.Clear();

        for (int i = 0; i < implementedInterfaces.Length; i++)
        {
            if (Ricci.Instance.KnowsInterface(implementedInterfaces[i]))
            {
                interfaceList.Add(implementedInterfaces[i].Name);

                if (implementedInterfaces[i] == typeof(IDamageable))
                {
                    showHealthBar = true;
                }
            }
        }

        if (showHealthBar)
            HUD.InfoPanel.Show(name, interfaceList.ToArray(), (IDamageable)this);
        else
            HUD.InfoPanel.Show(name, interfaceList.ToArray());
    }

    public void HideInterfaces()
    {
        HUD.InfoPanel.Hide();
    }

    public void SetSelected()
    {
        originalMaterial = meshRenderer.material;
        meshRenderer.material = Resources.Load<Material>(GlobalDefinitions.SelectedMaterialPath);

        DisplayInterfaces();

        HUD.Terminal.OnActorDeselection += Deselect;
    }

    private void Deselect()
    {
        meshRenderer.material = originalMaterial;
        originalMaterial = null;

        HideInterfaces();

        HUD.Terminal.OnActorDeselection -= Deselect;
    }

    ///<exception cref="MethodAccessException">Thrown when the method being called is not accessible</exception>
    ///<exception cref="NotImplementedException">Thrown when there is no implemented interface with the given name</exception>    
    ///<exception cref="MissingMethodException">Thrown when there is no method with the given name</exception>
    ///<exception cref="TargetParameterCountException">Thrown when the target method has no overload with the given parameter count</exception>    
    public void SubmitCommand(Command command)
    {
        //Checks if the current Type implements the given Type
        //Will proceed to call the given Method if it is implemented from the given interface
        if (command.abstractionInterfaceType.IsAssignableFrom(GetType()))
        {
            MethodInfo[] implementedMethods = command.abstractionInterfaceType.GetMethods();

            foreach (MethodInfo m in implementedMethods)
            {
                //Checks if this Command's Method name is found within the interface
                //If it is, checks parameters for correct override call
                if (m.Equals(command.methodInfo))
                {
                    if (m.IsPublic)
                    {
                        try
                        {                            
                            m.Invoke(this, command.parameters);
                        }
                        catch (TargetParameterCountException t)
                        {
                            throw t;
                        }                        

                        return;
                    }
                    else
                        throw new MethodAccessException();
                }
            }

            throw new MissingMethodException();
        }
        else
            throw new NotImplementedException();
    }

    public void SubmitSpell(Spell spell)
    {
        foreach (Command c in spell.commands)
        {
            try
            {
                SubmitCommand(c);
            }
#pragma warning disable 0168
            catch (MethodAccessException mae)
            {
                HUD.Log.Push(GlobalDefinitions.InvalidMethodErrorMessage);
                break;
            }
            catch (NotImplementedException nie)
            {
                HUD.Log.Push(GlobalDefinitions.InvalidMethodErrorMessage);
                break;
            }
            catch (MissingMethodException mme)
            {
                HUD.Log.Push(GlobalDefinitions.InvalidMethodErrorMessage);
                break;   
            }
            catch (TargetParameterCountException tpce)
#pragma warning restore 0168
            {
                HUD.Log.Push(GlobalDefinitions.InvalidParametersErrorMessage);
                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.pointerId == -1)
        {
            HUD.Terminal.SetSelectedActor(this);
            SetSelected();
        }
    }

    public virtual void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        interfaceList = new List<string>();

        implementedInterfaces = GetType().GetInterfaces();
    }
}
