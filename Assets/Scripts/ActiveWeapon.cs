using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    
    private RaycastWeapon _weapon;
    private Animator anim;
    private AnimatorOverrideController overrideAnim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        overrideAnim = anim.runtimeAnimatorController as AnimatorOverrideController;
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_weapon)
        {
            if (Input.GetButton("Fire1"))
            {
                _weapon.StartFiring();
            }
            if (_weapon.isFiring)
            {
                _weapon.UpdateFiring(Time.deltaTime);
            }
            _weapon.UpdateBullet(Time.deltaTime);
            if (Input.GetButtonUp("Fire1"))
            {
                _weapon.StopFiring();
            }

        }
        else
        {
            handIK.weight = 0f;
            anim.SetLayerWeight(1, 0f);
        }
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        if (_weapon)
        {
            Destroy(_weapon.gameObject);
        }
        
        _weapon = newWeapon;
        _weapon._raycastDestination = crossHairTarget;
        _weapon.transform.parent = weaponParent;
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;
        handIK.weight = 1f;
        anim.SetLayerWeight(1, 1f);
        Invoke(nameof(SetAnimationDelay), 0.01f);
    }

    // we need this because of a crash when this code is run in start or awake.
    void SetAnimationDelay()
    {
        overrideAnim["weapon_empty"] = _weapon.weaponAnimation;

    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0f);
        recorder.SaveToClip(_weapon.weaponAnimation);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}
