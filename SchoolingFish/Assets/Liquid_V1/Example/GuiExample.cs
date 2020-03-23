using UnityEngine;
using System.Collections;

public class GuiExample : MonoBehaviour {

	public Material[] liquidMaterial;
	
	private GameObject _liquid;
	private HTLiquidSpriteSheet liquidSpriteSheet;
	
	void Start(){
		_liquid = GameObject.Find("WaterPlane");	
		liquidSpriteSheet = _liquid.GetComponent<HTLiquidSpriteSheet>();
	}
	
	void OnGUI(){
		
		for (int i=0;i<liquidMaterial.Length/2;i++){
		
			//GUI.color = new Color(1f,0.75f,0.5f);
			if (GUI.Button(new Rect( 10,10+i*30,110,20),liquidMaterial[i].name)){
				_liquid.GetComponent<Renderer>().material = liquidMaterial[i];
				liquidSpriteSheet = _liquid.GetComponent<HTLiquidSpriteSheet>();
				liquidSpriteSheet.InitSpriteTexture();
			}
		}
		
		int j=0;
		for (int i=liquidMaterial.Length/2;i<liquidMaterial.Length;i++){
		
			if (GUI.Button(new Rect( Screen.width-120,10+j*30,110,20),liquidMaterial[i].name)){
				_liquid.GetComponent<Renderer>().material = liquidMaterial[i];
				liquidSpriteSheet = _liquid.GetComponent<HTLiquidSpriteSheet>();
				liquidSpriteSheet.InitSpriteTexture();
			}
			j++;
		}
		
		GUI.Label( new Rect(10,Screen.height-50,150,25),"Animation speed : " + liquidSpriteSheet.framesPerSecond);
		liquidSpriteSheet.framesPerSecond = (int)GUI.HorizontalSlider( new Rect(10,Screen.height-30,125,20), liquidSpriteSheet.framesPerSecond,1,100); 
		
		GUI.Label( new Rect(175,Screen.height-50,150,25),"X size: " + liquidSpriteSheet.textureSize.x);
		liquidSpriteSheet.textureSize.x = (int)GUI.HorizontalSlider( new Rect(175,Screen.height-30,125,20), liquidSpriteSheet.textureSize.x,1,500); 

		GUI.Label( new Rect(340,Screen.height-50,150,25),"Y size: " + liquidSpriteSheet.textureSize.y);
		liquidSpriteSheet.textureSize.y = (int)GUI.HorizontalSlider( new Rect(340,Screen.height-30,125,20), liquidSpriteSheet.textureSize.y,1,500); 

		GUI.Label( new Rect(505,Screen.height-50,150,25),"X scroll: " + liquidSpriteSheet.scrollSpeed.x);
		liquidSpriteSheet.scrollSpeed.x = GUI.HorizontalSlider( new Rect(505,Screen.height-30,125,20), liquidSpriteSheet.scrollSpeed.x,0,5); 
		
		GUI.Label( new Rect(670,Screen.height-50,150,25),"Y Scroll: " + liquidSpriteSheet.scrollSpeed.y);
		liquidSpriteSheet.scrollSpeed.y = GUI.HorizontalSlider( new Rect(670,Screen.height-30,125,20), liquidSpriteSheet.scrollSpeed.y,0,5); 
	}
}
