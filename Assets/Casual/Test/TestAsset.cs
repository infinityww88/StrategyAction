using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class TestAsset : MonoBehaviour
{
	// Start is called before the first frame update
	ResourcePackage package;
	public MeshRenderer renderer;

    void Start()
    {
	    YooAssets.Initialize();
	    
	    //YooAssets.SetDefaultPackage(package);
	    StartCoroutine(InitializeYooAsset());
    }
    
	public class BuildQuery : IBuildinQueryServices {
		public bool Query(string packageName, string fileName, string fileCRC) {
			return false;
		}
	}
	
	public class RemoteServ : IRemoteServices {
		
		public string GetRemoteMainURL(string fileName) {
			return $"http://127.0.0.1:9090/{fileName}";
		}

		public string GetRemoteFallbackURL(string fileName) {
			return $"http://127.0.0.1:9090/{fileName}";
		}
	}
	
	string packageVersion = null;
	
	private IEnumerator UpdatePackageVersion()
	{
		var package = YooAssets.GetPackage("AddPackage");
		var operation = package.UpdatePackageVersionAsync();
		yield return operation;

		if (operation.Status == EOperationStatus.Succeed)
		{
			//更新成功
			packageVersion = operation.PackageVersion;
			Debug.Log($"Updated package Version : {packageVersion}");
		}
		else
		{
			//更新失败
			Debug.LogError(operation.Error);
		}
	}
	
	private IEnumerator UpdatePackageManifest()
	{
		// 更新成功后自动保存版本号，作为下次初始化的版本。
		// 也可以通过operation.SavePackageVersion()方法保存。
		bool savePackageVersion = true;
		var package = YooAssets.GetPackage("AddPackage");
		var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
		yield return operation;

		if (operation.Status == EOperationStatus.Succeed)
		{
			//更新成功
		}
		else
		{
			//更新失败
			Debug.LogError(operation.Error);
		}
	}
    
	private IEnumerator InitializeYooAsset()
	{
		package = YooAssets.CreatePackage("AddPackage");
		YooAssets.SetDefaultPackage(package);
		
		//string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";
		//string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";
		var initParameters = new HostPlayModeParameters();
		initParameters.BuildinQueryServices = new BuildQuery(); 
		//initParameters.DecryptionServices = new FileOffsetDecryption();
		initParameters.RemoteServices = new RemoteServ();
		var initOperation = package.InitializeAsync(initParameters);
		yield return initOperation;
    
		if(initOperation.Status == EOperationStatus.Succeed)
		{
			Debug.Log("资源包初始化成功！");
		}
		else 
		{
			Debug.LogError($"资源包初始化失败：{initOperation.Error}");
		}
		
		yield return UpdatePackageVersion();
		yield return UpdatePackageManifest();
		
		var infos = package.GetAssetInfos("image");
		foreach (var e in infos) {
			Debug.Log($"{e.Address} {e.AssetPath} {e.AssetType}");
		}
		
		
		//var handle = package.LoadAssetAsync("UVChecker");
		//yield return handle;
		
		var handle = package.LoadAssetAsync<Texture>("NewTexture");
		yield return handle;
		if (handle.LastError != null && handle.LastError.Length > 0) {
			Debug.Log(handle.LastError);
		}
		else {
			var t = handle.AssetObject as Texture;
			renderer.material.SetTexture("_MainTex", t);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
