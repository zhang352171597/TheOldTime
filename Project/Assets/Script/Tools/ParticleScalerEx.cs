using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ParticleScalerEx : MonoBehaviour {
//	void OnWillRenderObject(){
//		GetComponent<Renderer>().material.SetVector("_Position",Camera.current.worldToCameraMatrix.MultiplyPoint(transform.root.position));
//		GetComponent<Renderer>().material.SetVector("_Scale",transform.lossyScale);
//	}

	[Header("测试"), SerializeField]
	bool isTest;
	[SerializeField,Header("测试缩放大小")]
	float testScale;

	[SerializeField]Transform[] particle;
	[SerializeField]
	bool keyUpdate = true;

	[SerializeField, Header("粒子网格特效")]
	ParticleSystem[] particleEx;
	Dictionary<ParticleSystem, float> particleExDict = new Dictionary<ParticleSystem, float>();


	void Awake(){
	}

	#region Interface

	public void SetScale(float scale){
		Vector3 newscale = Vector3.one * scale;
		SetScale (newscale);
	}
	public void SetScale(Vector3 _scale){
		_getAllChild();
		for (int i = 0; i < particle.Length; i++) {
			if (particle [i] == null)
				continue;
			particle [i].transform.localScale = _scale;
		}
		scaleParticleEx (_scale.x);
	}

	#endregion

	#region Private

    void _getAllChild()
    {
        if (particle == null)
        {
            keyUpdate = false;
            particle = new Transform[transform.childCount];
            for (int i = 0; i < particle.Length; ++i)
            {
                particle[i] = transform.GetChild(i);
            }
        }
    }

	void scaleParticleEx(float _scale){
		for(int i = 0;i < particleEx.Length;i++){
			if (particleEx [i] == null)
				continue;
			var particleComponent = particleEx [i].GetComponent<ParticleSystem> ();
			if (particleComponent == null)
				continue;
			if(!particleExDict.ContainsKey(particleComponent))
				particleExDict.Add(particleComponent, particleComponent.startSize);

			particleComponent.startSize = _scale * particleExDict [particleComponent];
			particleComponent.scalingMode = ParticleSystemScalingMode.Shape;
		}
	}

	void Update(){
		if (!Application.isPlaying && isTest) {
			testUpdateScale ();
		} 
//		else if (keyUpdate) {
//
//			SetScale (transform.localScale.x);
//		}


	}

	void testUpdateScale(){
		SetScale (testScale);
	}

	#endregion

}