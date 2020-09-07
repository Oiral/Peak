//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: This object's rigidbody goes to sleep when created
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class SleepOnAwake : MonoBehaviour
	{
		//-------------------------------------------------
		void Awake()
		{
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			if ( rigidbody != null )
			{
                Sleep(rigidbody);
			}
		}

        private void FixedUpdate()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
            {
                if (rigidbody.IsSleeping())
                {
                    Destroy(this);
                }
                else
                {
                    Sleep(rigidbody);
                }
            }
        }

        void Sleep(Rigidbody rigidbody)
        {
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            rigidbody.Sleep();
        }
    }
}
