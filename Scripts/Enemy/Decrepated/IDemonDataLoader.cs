using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("No longer needed to load creature data")]
public interface IDemonDataLoader
{
    public List<float> LoadDemonData();

    public List<Texture> LoadDemonSprites();
}
