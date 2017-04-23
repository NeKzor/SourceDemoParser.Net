using System.Collections.Generic;

namespace SourceDemoParser.Net
{
	public struct SupportedGames
	{
		public static readonly Game Unknown = new Game("Unknown");
		public static readonly Game Portal = new Game("Portal", 66, new List<Map>
		{
			new Map("testchmb_a_00", "Ch. 0/1"),
			new Map("testchmb_a_01", "Ch. 2/3"),
			new Map("testchmb_a_02", "Ch. 4/5"),
			new Map("testchmb_a_03", "Ch. 6/7"),
			new Map("testchmb_a_04", "Ch. 8"),
			new Map("testchmb_a_05", "Ch. 9"),
			new Map("testchmb_a_06", "Ch. 10"),
			new Map("testchmb_a_07", "Ch. 11/12"),
			new Map("testchmb_a_08", "Ch. 13"),
			new Map("testchmb_a_09", "Ch. 14"),
			new Map("testchmb_a_10", "Ch. 15"),
			new Map("testchmb_a_11", "Ch. 16"),
			new Map("testchmb_a_13", "Ch. 17"),
			new Map("testchmb_a_14", "Ch. 18"),
			new Map("testchmb_a_15", "Ch. 19"),
			new Map("escape_00", "Ch. Escape 0"),
			new Map("escape_01", "Ch. Escape 1"),
			new Map("escape_02", "Ch. Escape 2")
		});
		public static readonly Game Portal2SinglePlayer = new Game("Portal 2", "Single Player", 60, new[] { "sp_a1_intro1", "sp_a1_intro2", "sp_a1_intro3", "sp_a1_intro4", "sp_a1_intro5", "sp_a1_intro6", "sp_a1_intro7", "sp_a1_wakeup", "sp_a2_intro", "sp_a2_laser_intro", "sp_a2_laser_stairs", "sp_a2_dual_lasers", "sp_a2_laser_over_goo", "sp_a2_catapult_intro", "sp_a2_trust_fling", "sp_a2_pit_flings", "sp_a2_fizzler_intro", "sp_a2_sphere_peek", "sp_a2_ricochet", "sp_a2_bridge_intro", "sp_a2_bridge_the_gap", "sp_a2_turret_intro", "sp_a2_laser_relays", "sp_a2_turret_blocker", "sp_a2_laser_vs_turret", "sp_a2_pull_the_rug", "sp_a2_column_blocker", "sp_a2_laser_chaining", "sp_a2_triple_laser", "sp_a2_bts1", "sp_a2_bts2", "sp_a2_bts3", "sp_a2_bts4", "sp_a2_bts5", "sp_a2_bts6", "sp_a2_core", "sp_a3_00", "sp_a3_01", "sp_a3_03", "sp_a3_jump_intro", "sp_a3_bomb_flings", "sp_a3_crazy_box", "sp_a3_transition01", "sp_a3_speed_ramp", "sp_a3_speed_flings", "sp_a3_portal_intro", "sp_a3_end", "sp_a4_intro", "sp_a4_tb_intro", "sp_a4_tb_trust_drop", "sp_a4_tb_wall_button", "sp_a4_tb_polarity", "sp_a4_tb_catch", "sp_a4_stop_the_box", "sp_a4_laser_catapult", "sp_a4_laser_platform", "sp_a4_speed_tb_catch", "sp_a4_jump_polarity", "sp_a4_finale1", "sp_a4_finale2", "sp_a4_finale3", "sp_a4_finale4" }, new[] { "Container Ride", "Portal Carousel", "Portal Gun", "Smooth Jazz", "Cube Momentum", "Future Starter", "Secret Panel", "Wakeup", "Incinerator", "Laser Intro", "Laser Stairs", "Dual Lasers", "Laser over Goo", "Catapult Intro", "Trust Fling", "Pit Flings", "Fizzler Intro", "Ceiling Catapult", "Ricochet", "Bridge Intro", "Bridge the Gap", "Turret Intro", "Laser Relays", "Turret Blocker", "Laser vs Turret", "Pull the Rug", "Column Blocker", "Laser Chaining", "Triple Laser", "Jail Break", "Escape", "Turret Factory", "Turret Sabotage", "Neurotoxin Sabotage", "Tube Ride", "Core", "The Fall", "Underground", "Cave Johnson", "Repulsion Intro", "Bomb Flings", "Crazy Box", "PotatOS", "Propulsion Intro", "Propulsion Flings", "Conversion Intro", "Three Gels", "Test", "Funnel Intro", "Ceiling Button", "Wall Button", "Polarity", "Funnel Catch", "Stop the Box", "Laser Catapult", "Laser Platform", "Propulsion Catch", "Repulsion Polarity", "Finale 1", "Finale 2", "Finale 3", "Finale 4" }); // ITS CALLED STANDARD MODE
		public static readonly Game Portal2Cooperative = new Game("Portal 2", "Cooperative", 60, new[] { "mp_coop_start", "mp_coop_lobby_3", "mp_coop_doors", "mp_coop_race_2", "mp_coop_laser_2", "mp_coop_rat_maze", "mp_coop_laser_crusher", "mp_coop_teambts", "mp_coop_lobby_3", "mp_coop_fling_3", "mp_coop_infinifling_train", "mp_coop_come_along", "mp_coop_fling_1", "mp_coop_catapult_1", "mp_coop_multifling_1", "mp_coop_fling_crushers", "mp_coop_fan", "mp_coop_lobby_3", "mp_coop_wall_intro", "mp_coop_wall_2", "mp_coop_catapult_wall_intro", "mp_coop_wall_block", "mp_coop_catapult_2", "mp_coop_turret_walls", "mp_coop_turret_ball", "mp_coop_wall_5", "mp_coop_lobby_3", "mp_coop_tbeam_redirect", "mp_coop_tbeam_drill", "mp_coop_tbeam_catch_grind_1", "mp_coop_tbeam_laser_1", "mp_coop_tbeam_polarity", "mp_coop_tbeam_polarity2", "mp_coop_tbeam_polarity3", "mp_coop_tbeam_maze", "mp_coop_tbeam_end", "mp_coop_lobby_3", "mp_coop_paint_come_along", "mp_coop_paint_redirect", "mp_coop_paint_bridge", "mp_coop_paint_walljumps", "mp_coop_paint_speed_fling", "mp_coop_paint_red_racer", "mp_coop_paint_speed_catch", "mp_coop_paint_longjump_intro" }, new[] { "Calibration", "Lobby 1", "Doors", "Buttons", "Lasers", "Rat Maze", "Laser Crushers", "Behind the Scenes", "Lobby 2", "Flings", "Infinifling", "Team Retrieval", "Vertical Flings", "Catapults", "Multifling", "Fling Crushers", "Industrial Fan", "Lobby 3", "Cooperative Bridges", "Bridge Swap", "Fling Block", "Catapult Block", "Bridge Fling", "Turret Walls", "Turret Assassin", "Bridge Testing", "Lobby 4", "Cooperative Funnels", "Funnel Drill", "Funnel Catch", "Funnel Laser", "Cooperative Polarity", "Funnel Hop", "Advanced Polarity", "Funnel Maze", "Turret Warehouse", "Lobby 5", "Repulsion Jumps", "Double Bounce", "Bridge Repulsion", "Wall Repulsion", "Propulsion Crushers", "Turret Ninja", "Propulsion Retrieval", "Vault Entrance" });
		public static readonly Game Portal2CooperativeDlc = new Game("Portal 2", "Cooperative", 60, new[] { "mp_coop_lobby_3", "mp_coop_separation_1", "mp_coop_tripleaxis", "mp_coop_catapult_catch", "mp_coop_2paints_1bridge", "mp_coop_paint_conversion", "mp_coop_bridge_catch", "mp_coop_laser_tbeam", "mp_coop_paint_rat_maze", "mp_coop_paint_crazy_box" }, new[] { "Lobby", "Separation", "Triple Axis", "Catapult Catch", "Bridge Gels", "Maintenance", "Bridge Catch", "Double Lift", "Gel Maze", "Crazier Box" });
		public static readonly Game Portal2Workshop = new Game("Portal 2", "Workshop", 60);
		public static readonly Game ApertureTag = new Game("Aperture Tag", "Single Player", 60,
			new List<Map>
			{
				new Map("gg_intro_wakeup"),
				new Map("gg_blue_only"),
				new Map("gg_blue_only_2"),
				new Map("gg_blue_only_3"),
				new Map("gg_blue_only_2_pt2"),
				new Map("gg_a1_intro4"),
				new Map("gg_blue_upplatform"),
				new Map("gg_red_only"),
				new Map("gg_red_surf"),
				new Map("gg_all_intro"),
				new Map("gg_all_rotating_wall"),
				new Map("gg_all_fizzler"),
				new Map("gg_all_intro_2"),
				new Map("gg_a2_column_blocker"),
				new Map("gg_all_puzzle2"),
				new Map("gg_all2_puzzle1"),
				new Map("gg_all_puzzle1"),
				new Map("gg_all2_escape"),
				new Map("gg_stage_reveal"),
				new Map("gg_stage_bridgebounce_2"),
				new Map("gg_stage_redfirst"),
				new Map("gg_stage_laserrelay"),
				new Map("gg_stage_beamscotty"),
				new Map("gg_stage_bridgebounce"),
				new Map("gg_stage_roofbounce"),
				new Map("gg_stage_pickbounce"),
				new Map("gg_stage_theend"),
				new Map("gg_tag_remix"),
				new Map("gg_trailer_map")
			});
		public static readonly Game ApertureTagWorkshop = new Game("Aperture Tag", "Workshop", 60);
		public static readonly Game PortalStoriesMel = new Game("Portal Stories: Mel", 60,
			new List<Map>
			{
				new Map("sp_a1_tramride"),
				new Map("sp_a1_mel_intro"),
				new Map("sp_a1_lift"),
				new Map("sp_a1_garden"),
				new Map("sp_a2_garden_de"),
				new Map("sp_a2_underbounce"),
				new Map("sp_a2_once_upon"),
				new Map("sp_a2_past_power"),
				new Map("sp_a2_ramp"),
				new Map("sp_a2_firestorm"),
				new Map("sp_a3_junkyard"),
				new Map("sp_a3_concepts"),
				new Map("sp_a3_paint_fling"),
				new Map("sp_a3_faith_plate"),
				new Map("sp_a3_transition"),
				new Map("sp_a4_overgrown"),
				new Map("sp_a4_tb_over_goo"),
				new Map("sp_a4_two_of_a_kind"),
				new Map("sp_a4_destroyed"),
				new Map("sp_a4_factory"),
				new Map("sp_a4_core_access"),
				new Map("sp_a4_finale"),
				new Map("st_a1_tramride"),
				new Map("st_a1_mel_intro"),
				new Map("st_a1_lift"),
				new Map("st_a1_garden"),
				new Map("st_a2_garden_de"),
				new Map("st_a2_underbounce"),
				new Map("st_a2_once_upon"),
				new Map("st_a2_past_power"),
				new Map("st_a2_ramp"),
				new Map("st_a2_firestorm"),
				new Map("st_a3_junkyard"),
				new Map("st_a3_concepts"),
				new Map("st_a3_paint_fling"),
				new Map("st_a3_faith_plate"),
				new Map("st_a3_transition"),
				new Map("st_a4_overgrown"),
				new Map("st_a4_tb_over_goo"),
				new Map("st_a4_two_of_a_kind"),
				new Map("st_a4_destroyed"),
				new Map("st_a4_factory"),
				new Map("st_a4_core_access"),
				new Map("st_a4_finale")
			});
		public static readonly Game Infra = new Game("INFRA", "Single Player", 120,
			new List<Map>
			{
				new Map("infra_c1_m1_office"),
				new Map("infra_c2_m1_reserve1"),
				new Map("infra_c2_m2_reserve2"),
				new Map("infra_c2_m3_reserve3"),
				new Map("infra_c3_m1_tunnel"),
				new Map("infra_c3_m2_tunnel2"),
				new Map("infra_c3_m3_tunnel3"),
				new Map("infra_c3_m4_tunnel4"),
				new Map("infra_c4_m2_furnace"),
				new Map("infra_c4_m3_tower"),
				new Map("infra_c5_m1_watertreatment"),
				new Map("infra_c5_m2_sewer"),
				new Map("infra_c5_m2b_sewer2"),
				new Map("infra_c6_m1_sewer3"),
				new Map("infra_c6_m2_metro"),
				new Map("infra_c6_m3_metroride"),
				new Map("infra_c6_m4_waterplant"),
				new Map("infra_c6_m5_minitrain"),
				new Map("infra_c6_m6_central"),
				new Map("infra_c7_m1_servicetunnel"),
				new Map("infra_c7_m1b_skyscraper"),
				new Map("infra_c7_m2_bunker"),
				new Map("infra_c7_m3_stormdrain"),
				new Map("infra_c7_m4_cistern"),
				new Map("infra_c7_m5_powerstation"),
				new Map("infra_ee_hallway"),
				new Map("main_menu")
			});
		public static readonly Game InfraWorkshop = new Game("INFRA", "Workshop", 120);
	}
}