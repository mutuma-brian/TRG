﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	public class Dpad : MonoBehaviour, ISimpleInputDraggable
	{
		public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput( "Horizontal" );
		public SimpleInput.AxisInput yAxis = new SimpleInput.AxisInput( "Vertical" );

		public float valueMultiplier = 1f;

#pragma warning disable 0649
		[Tooltip( "Radius of the deadzone at the center of the Dpad that will yield no input" )]
		[SerializeField]
		private float deadzoneRadius = 20f;
		private float deadzoneRadiusSqr;
#pragma warning restore 0649

		private RectTransform rectTransform;

		private Vector2 m_value = Vector2.zero;
		public Vector2 Value { get { return m_value; } }

		private void Awake()
		{
			rectTransform = (RectTransform) transform;
			gameObject.AddComponent<SimpleInputDragListener>().Listener = this;

			deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
		}

		private void OnEnable()
		{
			xAxis.StartTracking();
			yAxis.StartTracking();
		}

		private void OnDisable()
		{
			xAxis.StopTracking();
			yAxis.StopTracking();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
		}
#endif

		public void OnPointerDown( PointerEventData eventData )
		{
			CalculateInput( eventData );
		}

		public void OnDrag( PointerEventData eventData )
		{
			CalculateInput( eventData );
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			m_value = Vector2.zero;

			xAxis.value = 0f;
			yAxis.value = 0f;
		}

		private void CalculateInput( PointerEventData eventData )
		{
			Vector2 pointerPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle( rectTransform, eventData.position, eventData.pressEventCamera, out pointerPos );

			if( pointerPos.sqrMagnitude <= deadzoneRadiusSqr )
				m_value.Set( 0f, 0f );
			else
			{
				float angle = Vector2.Angle( pointerPos, Vector2.right );
				if( pointerPos.y < 0f )
					angle = 360f - angle;

				if( angle >= 25f && angle <= 155f )
					m_value.y = valueMultiplier;
				else if( angle >= 205f && angle <= 335f )
					m_value.y = -valueMultiplier;
				else
					m_value.y = 0f;

				if( angle <= 65f || angle >= 295f )
					m_value.x = valueMultiplier;
				else if( angle >= 115f && angle <= 245f )
					m_value.x = -valueMultiplier;
				else
					m_value.x = 0f;
			}

			xAxis.value = m_value.x;
			yAxis.value = m_value.y;
		}
	}
}