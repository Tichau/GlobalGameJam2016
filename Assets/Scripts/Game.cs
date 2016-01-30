using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public Level Level;

	private void Start ()
	{
	    this.Level.Start();
	}
	
	private void Update ()
	{
	    this.Level.Update();
	}
}
