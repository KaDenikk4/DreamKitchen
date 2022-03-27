using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    private static PersistenceManager instance;
    
    private BinaryFormatter bf = new BinaryFormatter();

    private GameManager gm;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }
    }


    public GameData CreateSaveGameData()
    {
        GameData saveDataObject = new GameData();

        return saveDataObject;
    }

    public void SaveGame()
    {
        GameData saveData = CreateSaveGameData();

        KitchenProductSaveData tempStruct = new KitchenProductSaveData();
        
        for (int i = 0; i < gm.GetProductDatabase().Count; i++)
        {
            tempStruct.productObjectName   = gm.GetProductDatabase()[i].product.name;
            tempStruct.productEquipped     = gm.GetProductDatabase()[i].productEquipped;
            tempStruct.productPurchased    = gm.GetProductDatabase()[i].productPurchased;
            tempStruct.productUnlocked     = gm.GetProductDatabase()[i].productUnlocked;
            //
            saveData.productDatabaseToSave.Add(tempStruct);
        }

        saveData.standardCurrency = gm.GetStandardCurrency();
        saveData.premiumCurrency  = gm.GetPremiumCurrency();

        if (gm.playerEquipment.equippedKnife)
        {
            saveData.equippedKnife = gm.playerEquipment.equippedKnife.productName;    
        }

        
        
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
        bf.Serialize(file, saveData);
        
        file.Close();
        //ToDo: To save data you need to get corresponding data saved like line below:
        //ToDo: saveData.variable = gameObject.GetComponent<someDatabase>().variables;

        
        
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData saveDataObject = (GameData)bf.Deserialize(file);
            file.Close();
            ProductHolder tempStruct = new ProductHolder();
            
            
            //ToDo: To load data you need to get corresponding data populated below with something like:
            //ToDo: gameObject.GetComponent<someDatabase>().variables = saveDataObject.variables;
            
            for (int i = 0; i < gm.GetProductDatabase().Count; i++)
            {
                tempStruct = gm.GetProductDatabase()[i];
                
                if (saveDataObject.productDatabaseToSave[i].productObjectName == tempStruct.product.name)
                {
                    tempStruct.productEquipped     = saveDataObject.productDatabaseToSave[i].productEquipped;
                    tempStruct.productPurchased    = saveDataObject.productDatabaseToSave[i].productPurchased;
                    tempStruct.productUnlocked     = saveDataObject.productDatabaseToSave[i].productUnlocked;    
                }

                if (gm.GetKitchenProductDictionary().TryGetValue(saveDataObject.equippedKnife, out KitchenProduct equippedKnife))
                {
                    gm.playerEquipment.equippedKnife = equippedKnife;
                }

                gm.SetStandardCurrency(saveDataObject.standardCurrency);
                gm.SetPremiumCurrency(saveDataObject.premiumCurrency);
                
                gm.GetProductDatabase()[i] = tempStruct;
            }
        }
        else
        {
            Debug.Log("No save file found.");
        }
        
        Debug.Log("Game Loaded");
        
    }
}
