﻿using DD_WorkTab.Base;
using DD_WorkTab.Miscellaneous;
using DD_WorkTab.Tools;
using UnityEngine;
using Verse;

namespace DD_WorkTab.Primaries
{
	public class PrimaryWork
	{
		private WorkTypeInfo workInfo;

		private bool ctrlState;

		public readonly WorkTypeDef def;

		public Rect drawRect;

		private void OnShiftClick(int clickInt)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				Controller.GetManager.GetPawnSurface(pawn).OnPrimaryShiftClick(clickInt, this.def);
			}

			string shiftClickDone = "DD_WorkTab_Message_PrimaryShiftClick".CachedTranslation(new string[] { this.def.labelShort });

			Utilities.UserFeedbackChain(WorkSound.TaskCompleted, shiftClickDone);
		}

		private void OnCtrlClick()
		{
			this.ctrlState = !this.ctrlState;

			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				Controller.GetManager.GetPawnSurface(pawn).OnPrimaryCtrlClick(this.ctrlState, this.def);
			}

			string message = (this.ctrlState ? "DD_WorkTab_Message_PrimaryCtrlEnabledAll" : "DD_WorkTab_Message_PrimaryCtrlDisabledAll").CachedTranslation(new string[] { this.def.labelShort });
			WorkSound sound = this.ctrlState ? WorkSound.WorkEnabled : WorkSound.WorkDisabled;

			Utilities.UserFeedbackChain(sound, message);
		}

		public void DrawTexture(Rect rect)
		{
			Utilities.DraggableOutline(rect, Utilities.MediumSkillColour);

			GUI.DrawTexture(rect.ContractedBy(2f), this.workInfo.texture64);
		}

		public void OnClicked()
		{
			if (Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					this.OnShiftClick(-1);
				}

				if (Event.current.button == 1)
				{
					this.OnShiftClick(1);
				}
			}

			else if (Event.current.control)
			{
				this.OnCtrlClick();
			}

			Event.current.Use();
		}

		public void OnHover(Rect rect, bool compareSkillsWindow)
		{
			Widgets.DrawHighlight(rect);

			TooltipHandler.TipRegion(rect, Utilities.PrimaryTooltip(this, compareSkillsWindow));
		}

		public PrimaryWork(WorkTypeDef def)
		{
			this.def = def;

			this.workInfo = Utilities.WorkDefAttributes[this.def];
		}
	}
}
