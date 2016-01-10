﻿using System;
using System.Windows.Forms;
using System.Threading;

namespace RazorEnhanced.UI
{
	internal partial class EnhancedMobileInspector : Form
	{
		private const string m_Title = "Enhanced Mobile Inspect";
		private Thread m_ProcessInfo;
		private Assistant.Mobile m_mobile;

		internal EnhancedMobileInspector(Assistant.Mobile mobileTarg)
		{
			InitializeComponent();
			MaximizeBox = false;
			m_mobile = mobileTarg;
			// general
			lName.Text = mobileTarg.Name.ToString();
			lSerial.Text = "0x" + mobileTarg.Serial.Value.ToString("X8");
			lMobileID.Text = "0x" + mobileTarg.Body.ToString("X4");
			lColor.Text = mobileTarg.Hue.ToString();
			lPosition.Text = mobileTarg.Position.ToString();
			// Details
			if (mobileTarg.Female)
				lSex.Text = "Female";
			else
				lSex.Text = "Male";
			lHits.Text = mobileTarg.Hits.ToString();
			lMaxHits.Text = mobileTarg.Hits.ToString();
			lNotoriety.Text = mobileTarg.Notoriety.ToString();

			switch (mobileTarg.Direction & Assistant.Direction.Mask)
			{
				case Assistant.Direction.North: lDirection.Text = "North"; break;
				case Assistant.Direction.South: lDirection.Text = "South"; break;
				case Assistant.Direction.West: lDirection.Text = "West"; break;
				case Assistant.Direction.East: lDirection.Text = "East"; break;
				case Assistant.Direction.Right: lDirection.Text = "Right"; break;
				case Assistant.Direction.Left: lDirection.Text = "Left"; break;
				case Assistant.Direction.Down: lDirection.Text = "Down"; break;
				case Assistant.Direction.Up: lDirection.Text = "Up"; break;
				default: lDirection.Text = "Undefined"; break;
			}

			if (mobileTarg.Poisoned)
				lFlagPoisoned.Text = "Yes";
			else
				lFlagPoisoned.Text = "No";

			if (mobileTarg.Warmode)
				lFlagWar.Text = "Yes";
			else
				lFlagWar.Text = "No";

			if (mobileTarg.Visible)
				lFlagHidden.Text = "No";
			else
				lFlagHidden.Text = "Yes";

			if (mobileTarg.IsGhost)
				lFlagGhost.Text = "Yes";
			else
				lFlagGhost.Text = "No";

			if (mobileTarg.Blessed)		// Yellow Hits
				lFlagBlessed.Text = "Yes";
			else
				lFlagBlessed.Text = "No";

			if (mobileTarg.Paralized)     
				lFlagParalized.Text = "Yes";
			else
				lFlagParalized.Text = "No";

			for (int i = 0; i < mobileTarg.ObjPropList.Content.Count; i++)
			{
				Assistant.ObjectPropertyList.OPLEntry ent = mobileTarg.ObjPropList.Content[i];
				if (i == 0)
					lName.Text = ent.ToString();
				else
				{
					string content = ent.ToString();
					listBoxAttributes.Items.Add(content);
				}
			}

			if (mobileTarg.ObjPropList.Content.Count == 0)
			{
				lName.Text = mobileTarg.Name.ToString();
			}


			if (mobileTarg == Assistant.World.Player)
			{
				listBoxAttributes.Items.Add("");
				listBoxAttributes.Items.Add("Weight: " + Assistant.World.Player.Weight);
				if (Assistant.World.Player.Expansion >= 3)
				{
					listBoxAttributes.Items.Add("Stat Cap: " + Assistant.World.Player.StatCap);
					listBoxAttributes.Items.Add("Followers: " + Assistant.World.Player.Followers);
					listBoxAttributes.Items.Add("Max Followers: " + Assistant.World.Player.FollowersMax);

					if (Assistant.World.Player.Expansion >= 4)
					{
						listBoxAttributes.Items.Add("Fire Resist: " + Assistant.World.Player.FireResistance);
						listBoxAttributes.Items.Add("Cold Resist: " + Assistant.World.Player.ColdResistance);
						listBoxAttributes.Items.Add("Poison Resist: " + Assistant.World.Player.PoisonResistance);
						listBoxAttributes.Items.Add("Energy Resist: " + Assistant.World.Player.EnergyResistance);
						listBoxAttributes.Items.Add("Luck: " + Assistant.World.Player.Luck);
						listBoxAttributes.Items.Add("Damage Minimum: " + Assistant.World.Player.DamageMin);
						listBoxAttributes.Items.Add("Damage Maximum: " + Assistant.World.Player.DamageMax);
						listBoxAttributes.Items.Add("Tithing points: " + Assistant.World.Player.Tithe);

						if (Assistant.World.Player.Expansion >= 5)
						{
							switch (Assistant.World.Player.Race)
							{
								case 1:
									listBoxAttributes.Items.Add("Race: Human");
									break;
								case 2:
									listBoxAttributes.Items.Add("Race: Elf");
									break;
								case 3:
									listBoxAttributes.Items.Add("Race: Gargoyle");
									break;
							}

							listBoxAttributes.Items.Add("Max Weight: " + Assistant.World.Player.MaxWeight);

							if (Assistant.World.Player.Expansion >= 6)
							{
								if (Assistant.World.Player.SwingSpeedIncrease > 0)
									listBoxAttributes.Items.Add("Swing Speed Increase: " + Assistant.World.Player.SwingSpeedIncrease);

								if (Assistant.World.Player.DamageChanceIncrease > 0)
									listBoxAttributes.Items.Add("Damage Chance Increase: " + Assistant.World.Player.DamageChanceIncrease);

								if (Assistant.World.Player.LowerReagentCost > 0)
									listBoxAttributes.Items.Add("Lower Reagent Cost: " + Assistant.World.Player.LowerReagentCost);

								if (Assistant.World.Player.HitPointsRegeneration > 0)
									listBoxAttributes.Items.Add("Hit Points Regeneration: " + Assistant.World.Player.HitPointsRegeneration);

								if (Assistant.World.Player.StaminaRegeneration > 0)
									listBoxAttributes.Items.Add("Stamina Regeneration: " + Assistant.World.Player.StaminaRegeneration);

								if (Assistant.World.Player.ManaRegeneration > 0)
									listBoxAttributes.Items.Add("Mana Regeneration: " + Assistant.World.Player.ManaRegeneration);

								if (Assistant.World.Player.ReflectPhysicalDamage > 0)
									listBoxAttributes.Items.Add("Reflect Physical Damage: " + Assistant.World.Player.ReflectPhysicalDamage);

								if (Assistant.World.Player.EnhancePotions > 0)
									listBoxAttributes.Items.Add("Enhance Potions: " + Assistant.World.Player.EnhancePotions);

								if (Assistant.World.Player.DefenseChanceIncrease > 0)
									listBoxAttributes.Items.Add("Defense Chance Increase: " + Assistant.World.Player.DefenseChanceIncrease);

								if (Assistant.World.Player.SpellDamageIncrease > 0)
									listBoxAttributes.Items.Add("Spell Damage Increase: " + Assistant.World.Player.SpellDamageIncrease);

								if (Assistant.World.Player.FasterCastRecovery > 0)
									listBoxAttributes.Items.Add("Faster Cast Recovery: " + Assistant.World.Player.FasterCastRecovery);

								if (Assistant.World.Player.FasterCasting > 0)
									listBoxAttributes.Items.Add("Faster Casting: " + Assistant.World.Player.FasterCasting);

								if (Assistant.World.Player.LowerManaCost > 0)
									listBoxAttributes.Items.Add("Lower Mana Cost: " + Assistant.World.Player.LowerManaCost);

								if (Assistant.World.Player.StrengthIncrease > 0)
									listBoxAttributes.Items.Add("Strength Increase: " + Assistant.World.Player.StrengthIncrease);

								if (Assistant.World.Player.DexterityIncrease > 0)
									listBoxAttributes.Items.Add("Dexterity Increase: " + Assistant.World.Player.DexterityIncrease);

								if (Assistant.World.Player.IntelligenceIncrease > 0)
									listBoxAttributes.Items.Add("Intelligence Increase: " + Assistant.World.Player.IntelligenceIncrease);

								if (Assistant.World.Player.HitPointsIncrease > 0)
									listBoxAttributes.Items.Add("Hit Points Increase: " + Assistant.World.Player.HitPointsIncrease);

								if (Assistant.World.Player.StaminaIncrease > 0)
									listBoxAttributes.Items.Add("Stamina Increase: " + Assistant.World.Player.StaminaIncrease);

								if (Assistant.World.Player.ManaIncrease > 0)
									listBoxAttributes.Items.Add("Mana Increase: " + Assistant.World.Player.ManaIncrease);

								if (Assistant.World.Player.MaximumHitPointsIncrease > 0)
									listBoxAttributes.Items.Add("Maximum Hit PointsIncrease: " + Assistant.World.Player.MaximumHitPointsIncrease);

								if (Assistant.World.Player.MaximumStaminaIncrease > 0)
									listBoxAttributes.Items.Add("Maximum Stamina Increase: " + Assistant.World.Player.MaximumStaminaIncrease);

								if (Assistant.World.Player.MaximumManaIncrease > 0)
									listBoxAttributes.Items.Add("Maximum Mana Increase: " + Assistant.World.Player.MaximumManaIncrease);
							}
						}
					}
				}
			}
			else
			{
				m_ProcessInfo = new Thread(ProcessInfoThread);
				m_ProcessInfo.Start();
            }
	}

		private void ProcessInfoThread()
		{
			AddAttributesToList("Fire Resist: " + GetAttribute("Fire Resist"));
			AddAttributesToList("Cold Resist: " + GetAttribute("Cold Resist"));
			AddAttributesToList("Poison Resist: " + GetAttribute(".Poison Resist"));
			AddAttributesToList("Energy Resist: " + GetAttribute("Energy Resist"));
			AddAttributesToList("Luck: " + GetAttribute("Luck"));

			if (GetAttribute("Swing Speed Increase") > 0)
				AddAttributesToList("Swing Speed Increase: " + GetAttribute("Swing Speed Increase"));

			if (GetAttribute("Damage Chance Increase") > 0)
				AddAttributesToList("Damage Chance Increase: " + GetAttribute("Damage Chance Increase"));

			if (GetAttribute("Lower Reagent Cost") > 0)
				AddAttributesToList("Lower Reagent Cost: " + GetAttribute("Lower Reagent Cost"));

			if (GetAttribute("Hit Points Regeneration") > 0)
				AddAttributesToList("Hit Points Regeneration: " + GetAttribute("Hit Points Regeneration"));

			if (GetAttribute("Stamina Regeneration") > 0)
				AddAttributesToList("Stamina Regeneration: " + GetAttribute("Stamina Regeneration"));

			if (GetAttribute("Mana Regeneration") > 0)
				AddAttributesToList("Mana Regeneration: " + GetAttribute("Mana Regeneration"));

			if (GetAttribute("Reflect Physical Damage") > 0)
				AddAttributesToList("Reflect Physical Damage: " + GetAttribute("Reflect Physical Damage"));

			if (GetAttribute("Enhance Potions") > 0)
				AddAttributesToList("Enhance Potions: " + GetAttribute("Enhance Potions"));

			if (GetAttribute("Defense Chance Increase") > 0)
				AddAttributesToList("Defense Chance Increase: " + GetAttribute("Defense Chance Increase"));

			if (GetAttribute("Spell Damage Increase") > 0)
				AddAttributesToList("Spell Damage Increase: " + GetAttribute("Spell Damage Increase"));

			if (GetAttribute("Faster Cast Recovery") > 0)
				AddAttributesToList("Faster Cast Recovery: " + GetAttribute("Faster Cast Recovery"));

			if (GetAttribute("Faster Casting") > 0)
				AddAttributesToList("Faster Casting: " + GetAttribute("Faster Casting"));

			if (GetAttribute("Lower Mana Cost") > 0)
				AddAttributesToList("Lower Mana Cost: " + GetAttribute("Lower Mana Cost"));

			if (GetAttribute("Strength Increase") > 0)
				AddAttributesToList("Strength Increase: " + GetAttribute("Strength Increase"));

			if (GetAttribute("Dexterity Increase") > 0)
				AddAttributesToList("Dexterity Increase: " + GetAttribute("Dexterity Increase"));

			if (GetAttribute("Intelligence Increase") > 0)
				AddAttributesToList("Intelligence Increase: " + GetAttribute("Intelligence Increase"));

			if (GetAttribute("Hit Points Increase") > 0)
				AddAttributesToList("Hit Points Increase: " + GetAttribute("Hit Points Increase"));

			if (GetAttribute("Stamina Increase") > 0)
				AddAttributesToList("Stamina Increase: " + GetAttribute("Stamina Increase"));

			if (GetAttribute("Mana Increase") > 0)
				AddAttributesToList("Mana Increase: " + GetAttribute("Mana Increase"));

			if (GetAttribute("Maximum Hit Points Increase") > 0)
				AddAttributesToList("Maximum Hit PointsIncrease: " + GetAttribute("Maximum HitPoints Increase"));

			if (GetAttribute("Maximum Stamina Increase") > 0)
				AddAttributesToList("Maximum Stamina Increase: " + GetAttribute("Maximum Stamina Increase"));

			if (GetAttribute("Maximum Mana Increase") > 0)
				AddAttributesToList("Maximum Mana Increase: " + GetAttribute("Maximum Mana Increase"));
		}

		private void AddAttributesToList(string value)
		{
			if (Assistant.Engine.Running)
			{
				listBoxAttributes.Invoke(new Action(() => listBoxAttributes.Items.Add(value)));
			}
		}

		private int GetAttribute(string attributename)
		{
			int attributevalue = 0;

			Assistant.Item itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Arms);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
            }

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Bracelet);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Cloak);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Earrings);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Gloves);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Head);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.InnerLegs);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.InnerTorso);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.LeftHand);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.MiddleTorso);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Neck);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.OuterLegs);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.OuterTorso);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Pants);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.RightHand);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Ring);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Shirt);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Shoes);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Unused_x9);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Unused_xF);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Waist);
			if (itemtocheck != null)
			{
				RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			return attributevalue;
        }

		private void razorButton1_Click(object sender, EventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }

			this.Close();
		}

		private void bNameCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lName.Text);
		}

		private void bSerialCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSerial.Text);
		}

		private void bItemIdCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMobileID.Text);
		}

		private void bColorCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lColor.Text);
		}

		private void bPositionCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lPosition.Text);
		}

		private void bContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSex.Text);
		}

		private void bRContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lHits.Text);
		}

		private void bAmountCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMaxHits.Text);
		}

		private void bLayerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lNotoriety.Text);
		}

		private void bOwnedCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lDirection.Text);
		}

		private void EnhancedMobileInspector_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }
		}
	}
}