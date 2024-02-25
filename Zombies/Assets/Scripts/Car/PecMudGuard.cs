using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pec4.car
{
	public class PecMudGuard : MonoBehaviour
	{
		public PecCarController carController; // car controller to get the steering angle

		private Quaternion m_OriginalRotation;


		private void Start()
		{
			m_OriginalRotation = transform.localRotation;
		}


		private void Update()
		{
			transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, carController.CurrentSteerAngle, 0);
		}
	}
}