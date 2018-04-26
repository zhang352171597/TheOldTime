using UnityEngine;
using System.Collections;

public class DataCenter : ModuleComponent<DataCenter>
{
    BackpackData _backpackData;
    public BackpackData backpackData
    {
        get
        {
            if (_backpackData == null)
                _backpackData = new BackpackData();
            return _backpackData;
        }
    }
    UserData _userData;
    public UserData userData
    {
        get
        {
            if (_userData == null)
                _userData = new UserData();
            return _userData;
        }
    }
    public void Load()
    {
        backpackData.Load();
        userData.Load();
    }
	
}
