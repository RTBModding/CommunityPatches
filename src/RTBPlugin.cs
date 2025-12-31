using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using RTBPlugins;

namespace hookdll;

public class hookdll : IPluginHeight, IPlugin
{
    public InputMethods InputMethod => InputMethods.MetersXZ;

    public int TIMER_WAIT_SUCCESS => 0;

    public int TIMER_WAIT_FAILED => 0;

    public int MaximumPairCount => 1000;

    public string Description => "SLX HOOKDLL - DO NOT USE";

    public string About => "ABOUT";


    public hookdll()
    {
        ScriptLoader.Initialize();
        throw new System.NotImplementedException();
        
    }

    public void AcceptNewProjectSettings()
    {
        throw new System.NotImplementedException();
    }

    public void AcceptProjectSettings()
    {
        throw new System.NotImplementedException();
    }

    public double Fetch(double latitude_or_z, double longitude_or_x)
    {
        throw new System.NotImplementedException();
    }

    public List<double> Fetch(List<LatLong> latitude_longitude_pairs)
    {
        throw new System.NotImplementedException();
    }

    public List<GameEngines> GetSupportedEngines()
    {
        return new List<GameEngines> { GameEngines.All };
    }

    public void Initialize()
    {
        //throw new System.NotImplementedException();
        //MessageBox.Show("Initialize");

    }

    public void Load(string filename)
    {
        //throw new System.NotImplementedException();
        //MessageBox.Show("Load");
    }

    public void RenderNewProjectSettings(System.Windows.Forms.Panel panel)
    {
        //throw new System.NotImplementedException();
    }

    public void RenderProjectSettings(System.Windows.Forms.Panel panel)
    {
        //throw new System.NotImplementedException();
    }

    public void Save(string filename)
    {
        //throw new System.NotImplementedException();
    }

    public bool ValidateNewProjectSettings(out string errorMessage)
    {
        throw new System.NotImplementedException();
        // errorMessage = "ValidateNewProjectSettings";
        // return true;
    }

    public bool ValidateProjectSettings(out string errorMessage)
    {
        throw new System.NotImplementedException();
        // errorMessage = "ValidateProjectSettings";
        // return true;
    }
}
