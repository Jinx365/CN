﻿#region LICENSE

// Copyright 2014-2015 Support
// Lulu.cs is part of Support.
// 
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// 
// Filename: Support/Support/Lulu.cs
// Created:  01/10/2014
// Date:     20/01/2015/11:20
// Author:   h3h3

#endregion

namespace Support.Plugins
{
    #region

    using System;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Support.Util;
    using ActiveGapcloser = Support.Util.ActiveGapcloser;

    #endregion

    public class Lulu : PluginBase
    {
        public Lulu()
        {
            Q = new Spell(SpellSlot.Q, 925);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 650); //shield
            R = new Spell(SpellSlot.R, 900);

            Q.SetSkillshot(0.25f, 60, 1450, false, SkillshotType.SkillshotLine);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.CastCheck(Target, "ComboQ"))
                {
                    Q.Cast(Target);
                }

                if (W.CastCheck(Target, "ComboW"))
                {
                    W.CastOnUnit(Target);
                }
            }

            if (HarassMode)
            {
                if (Q.CastCheck(Target, "HarassQ"))
                {
                    Q.Cast(Target);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
            {
                return;
            }

            if (W.CastCheck(gapcloser.Sender, "GapcloserW"))
            {
                W.CastOnUnit(gapcloser.Sender);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
            {
                return;
            }

            if (W.CastCheck(unit, "InterruptW"))
            {
                W.CastOnUnit(unit);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "使用 Q", true);
            config.AddBool("ComboW", "使用 W", true);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "使用 Q", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("GapcloserW", "使用 W 防止突进", true);

            config.AddBool("InterruptW", "使用 W 打断技能", true);
        }
    }
}