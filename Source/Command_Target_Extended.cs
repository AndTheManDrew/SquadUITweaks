using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;
using Verse;

namespace SquadUITweaks
{
    public class Command_Target_Extended : Command_Target
    {
        public Command_Target_Extended(Command_Target original)
            {
                defaultLabel = original.defaultLabel;
                defaultDesc = original.defaultDesc;
                targetingParams = original.targetingParams;
                hotKey = original.hotKey;
                icon = original.icon;
                action = original.action;
                disabled = original.disabled;
                disabledReason = original.disabledReason;
            }

        public override GizmoResult GizmoOnGUI(
            Vector2 topLeft,
            float   maxWidth)
            {
                CurrentTargetGizmos.Clear();
                foreach (var command in TraverseOffirstGizmos
                                        .GetValue<List<Gizmo>>()
                                        .Where(gizmo => gizmo is Command_VerbTarget command)
                                        .Cast<Command_VerbTarget>())
                    {
                        CurrentTargetGizmos.Add(command);
                    }
                return base.GizmoOnGUI(topLeft, maxWidth);
            }

        private static Traverse traverseOfFirstGizmos;

        public static Traverse TraverseOffirstGizmos
            {
                get
                    {
                        if (traverseOfFirstGizmos == null)
                            {
                                traverseOfFirstGizmos = Traverse.Create(typeof(GizmoGridDrawer)).Field("firstGizmos");
                            }

                        return traverseOfFirstGizmos;
                    }
            }

        public static List<Command_VerbTarget> CurrentTargetGizmos = new List<Command_VerbTarget>();
        public override void GizmoUpdateOnMouseover()
            {
                if (CurrentTargetGizmos == null || CurrentTargetGizmos.Count == 0)
                    {
                        return;
                    }
                foreach (var verbCommand in CurrentTargetGizmos)
                    {
                        verbCommand?.GizmoUpdateOnMouseover();
                    }
            }
    }
}
