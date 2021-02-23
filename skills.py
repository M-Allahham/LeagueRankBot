from lview import *
import math, itertools, time
from . import items

Version = "experimental version"
MissileToSpell = {}
Spells         = {}
ChampionSpells = {}

class SFlag:
	Targeted        = 1
	Line            = 2
	Cone            = 4
	Area            = 8
	
	CollideWindwall = 16
	CollideChampion = 32
	CollideMob      = 64
	
	
	CollideGeneric   = CollideMob      | CollideChampion | CollideWindwall
	SkillshotLine    = CollideGeneric  | Line
	
class Spell:
	def __init__(self, name, missile_names, flags, delay = 0.0):
		global MissileToSpell, Spells
		
		self.flags = flags
		self.name = name
		self.missiles = missile_names
		self.delay = delay
		Spells[name] = self
		for missile in missile_names:
			MissileToSpell[missile] = self
			
	delay    = 0.0
	flags    = 0
	name     = "?"
	missiles = []
	
ChampionSpells = {
    # aatrox
	"aatrox": [
		Spell("aatroxw",                ["aatroxw"],                               SFlag.CollideGeneric)
	],
    # ahri
	"ahri": [                                                                      
		Spell("ahriorbofdeception",     ["ahriorbmissile", "ahriorbreturn"],                        SFlag.Line | SFlag.CollideWindwall),
		Spell("ahriseduce",             ["ahriseducemissile"],                     SFlag.CollideGeneric)
	],
    # akali
	"akali": [
		Spell("akalie",                  ["akaliemis"],                                 SFlag.Line | SFlag.CollideWindwall)
	],
    # alistar
    # amumu
    "amumu":[
        Spell("bandagetoss",            ["sadmummybandagetoss"],                    SFlag.SkillshotLine)
    ],
    # anivia
    "anivia": [
		Spell("flashfrostspell",             ["flashfrostspell"],                       SFlag.Line | SFlag.CollideWindwall),
		Spell("crystallize",            [],                                        SFlag.Area, delay=0.25),
		Spell("glacialstorm",           [],                                        SFlag.Area)
	],
    # annie
    "annie": [                                                                     
		Spell("incinerate",                 [],                                        SFlag.Cone | SFlag.CollideWindwall),
		Spell("infernalguardian",                 [],                                        SFlag.Area)
	],
    # aphelios
    "aphelios": [
        Spell("calibrumq",              ["aphelioscalibrumq"],                      SFlag.SkillshotLine),
        Spell("apheliosr",              ["apheliosr"],                          SFlag.SkillshotLine)
    ],
    # ashe
    "ashe": [                           
		Spell("volley",           ["volleyattack", "volleyattackwithsound"], SFlag.SkillshotLine),
		Spell("enchantedcrystalarrow",  ["enchantedcrystalarrow"],                 SFlag.Line | SFlag.CollideWindwall | SFlag.CollideChampion)
	],
    # aurelion sol
    "aurelionsol": [
		Spell("aurelionsolq",           ["aurelionsolqmissile"],                   SFlag.SkillshotLine),
		Spell("aurelionsolr",           ["aurelionsolrbeammissile"],               SFlag.Line | SFlag.CollideWindwall)
	],
    # azir
    "azir": [
        Spell("azirw",                  ["azirsoldiermissile"],                     SFlag.Area),
        Spell("azirr",                  ["azirsoldierrmissile"],                     SFlag.Cone)
    ],
    # bard
    "bard": [
        Spell("bardq",                  ["bardqmissile"],                           SFlag.SkillshotLine)
    ],
    #blitzcrank
    "blitzcrank": [
		Spell("rocketgrab",           ["rocketgrabmissile"],                                 SFlag.SkillshotLine),
	],
    #brand
    "brand": [
		Spell("brandq",                 ["brandqmissile"],                         SFlag.SkillshotLine),
		Spell("brandw",                 [],                                        SFlag.Area)
	],
    #braum
    "braum": [
        Spell("braumq",                 ["braumqmissile"],                          SFlag.SkillshotLine),
        Spell("braumrwrapper",                 ["braumrmissile"],                          SFlag.SkillshotLine)
    ],
    #caitlyn
    "caitlyn": [
		Spell("caitlynpiltoverpeacemaker", ["caitlynpiltoverpeacemaker", "caitlynpiltoverpeacemaker2"],          SFlag.Line | SFlag.CollideWindwall),
		Spell("caitlynyordletrap",         ["caitlyntrap"],                                                                   SFlag.Area),
		Spell("caitlynentrapment",         ["caitlynentrapmentmissile"],                                         SFlag.SkillshotLine)
	],
    #camille
    #cassiopeia
    "cassiopeia": [
        Spell("cassiopeiaq",                ["cassiopeiaq"],                                     SFlag.Area),
        Spell("cassiopeiar",                [],                                    SFlag.Cone)
    ],
    #chogath
    "chogath": [                        
		Spell("rupture",                ["rupture"],                                        SFlag.Area, delay = 0.627),
		Spell("feralscream",            ["feralscream"],                                        SFlag.Cone | SFlag.CollideWindwall)
	],
    #corki
    "corki": [
        Spell("phosphorusbomb",                 ["phosphorusbombmissile"],              SFlag.Area),
        Spell("missilebarrage",                 ["missilebarragemissile", "missilebarragemissile2"],              SFlag.SkillshotLine)
    ],
    #darius
    "darius": [                           
		Spell("dariuscleave",           [],                   SFlag.Area | SFlag.CollideWindwall),
		Spell("dariusaxegrabcone",      ["dariusaxegrabcone"],              SFlag.SkillshotLine)
	],
    #diana
    "diana": [
		Spell("dianaq",                 ["dianaqinnermissile", "dianaqoutermissile"], SFlag.Area)
	],
    #dr mundo
    "drmundo": [
		Spell("infectedcleavermissilecast", ["infectedcleavermissile"],            SFlag.SkillshotLine)
	],
    #draven
    "draven": [
        Spell("dravenrcast",                ["dravenr"],                    SFlag.SkillshotLine)
    ],
    #ekko
    "ekko": [
		Spell("ekkoq",                  ["ekkoqmis", "ekkoqreturn"],                              SFlag.Line | SFlag.CollideChampion),
		Spell("ekkow",                  ["ekkowmis"],                              SFlag.Area, delay=3.0),
        Spell("ekkor",                  ["ekkor"],                              SFlag.Area, delay=1.0)
	],
    #elise
    "elise": [
        Spell("elisehumane",            ["elisehumane"],                        SFlag.SkillshotLine)
    ],
    #evelynn
    "evelynn": [
		Spell("evelynnq",               ["evelynnq"],                              SFlag.SkillshotLine),
        Spell("evelynnr",               ["evelynnr"],                              SFlag.Area)
	],
    #ezreal
    "ezreal": [                         
		Spell("ezrealq",                ["ezrealq"],                               SFlag.SkillshotLine),
		Spell("ezrealw",                ["ezrealw"],                               SFlag.SkillshotLine),
		Spell("ezrealr",                ["ezrealr"],                               SFlag.SkillshotLine)
	],
    #fiddlesticks
    #fiora
    "fiora": [
        Spell("fioraw",                 [],                         SFlag.SkillshotLine)
    ],
    #fizz
    "fizz": [
		Spell("fizzr",                  ["fizzrmissile"],                          SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall)
	],
    #galio
    "galio": [
		Spell("galioq", [], SFlag.Area, delay=0.2),
		Spell("galioe", ["galioe"], SFlag.SkillshotLine, delay=0.3),
		Spell("galior", ["galiormissile"], SFlag.Area)
	],
    #gangplank
    #garen
    #gnar
    "gnar": [
        Spell("gnarbigq",           ["gnarbigqmissile"],        SFlag.SkillshotLine),
        Spell("gnarq",              ["gnarqmissile", "gnarqmissilereturn"],           SFlag.SkillshotLine),
        Spell("gnarbigw",           [],             SFlag.SkillshotLine)
    ],
    #gragas
    "gragas": [
        Spell("gragasq",        ["gragasqmissile"],         SFlag.Area),
        Spell("gragase",        ["gragase"],            SFlag.SkillshotLine),
        Spell("gragasr",        ["gragasr"],        SFlag.Area)
    ],
    #graves
    "graves": [                         
		Spell("gravesqlinespell",       ["gravesqlinemis", "gravesqreturn"],       SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall),
		Spell("gravessmokegrenade",     ["gravessmokegrenadeboom"],                SFlag.Area | SFlag.CollideWindwall),
		Spell("graveschargeshot",       ["graveschargeshotshot"],                  SFlag.Line | SFlag.CollideWindwall),
		Spell("graveschargeshotfxmissile2",       ["graveschargeshotfxmissile2"],                  SFlag.Line | SFlag.CollideWindwall)
	],
    #hecarim
    "hecarim": [
        Spell("hecarimultmissile",      ["HecarimUltMissileSkn4R1", "HecarimUltMissileSkn4c", "HecarimUltMissileSkn4R2", "HecarimUltMissileSKn4L2", "HecarimUltMissileSkn4L1"], SFlag.SkillshotLine)
    ],
    #heimerdinger
    "heimerdinger": [
        Spell("heimerdingerw", ["heimerdingerwattack2", "heimerdingerwattack2ult"],         SFlag.SkillshotLine),
        Spell("heimerdingere", ["heimerdingerespell", "heimerdingerespell_ult", "heimerdingerespell_ult2", "heimerdingerespell_ult3"], SFlag.Area)
    ],
    #illaoi
    "illaoi": [
		Spell("illaoiq",                ["illaoiq"],                                        SFlag.Area),
		Spell("illaoie",                ["illaoiemis"],                            SFlag.SkillshotLine),
        Spell("illaoir",                [],                 SFlag.Area)
	],
    #irelia
    "irelia": [
		Spell("ireliae2",                ["ireliaemissile"],                        SFlag.Area),
		Spell("ireliar",                ["ireliar"],                               SFlag.SkillshotLine)
	],
    #ivern
    "ivern": [
        Spell("ivernq",     [],     SFlag.SkillshotLine)
    ],
    #janna
    "janna": [
        Spell("howlinggale",     ["howlinggalespell"],     SFlag.SkillshotLine)
    ],
    #j4
    "jarvaniv": [
		Spell("jarvanivdemacianstandard",   [],                            SFlag.Area),
        Spell("jarvanivdragonstrike",                [],                        SFlag.SkillshotLine),
		Spell("jarvanivqe", [],                                       SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall)
	],
    #jax
    #jayce
    "jayce": [
        Spell("jayceq",     ["jayceshockblastmis", "jayceshockblastwallmis"],         SFlag.SkillshotLine)
    ],
    #jhin
    "jhin": [                           
		Spell("jhinw",                  ["jhinw"],                                 SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall, delay=0.5),
		Spell("jhine",                  ["jhinetrap"],                             SFlag.Area | SFlag.CollideWindwall),
		Spell("jhinrshot",              ["jhinrshotmis", "jhinrshotmis4"],         SFlag.Line | SFlag.CollideWindwall | SFlag.CollideChampion)
	],
    #jinx
    "jinx": [
        Spell("jinxr",      ["jinxrwrapper"],     SFlag.SkillshotLine),
        Spell("jinxwmissile", 			["jinxwmissile"], 			SFlag.SkillshotLine)
    ],
    #kaisa
    "kaisa": [
		Spell("kaisaw",                 ["kaisaw"],                         SFlag.Line | SFlag.CollideWindwall)
	],
    #kalista
    #karma
    #karthus
    #kassadin
    #katarina
    #kayle
    "kayle": [
		Spell("kayleq",                 ["kayleqmis"],                             SFlag.SkillshotLine)
	],
    #kayn
    "kayn": [
		Spell("kaynw", 			[], 			SFlag.SkillshotLine),
		Spell("kaynassw", 			[], 			SFlag.SkillshotLine),
	],
    #kennen
    #khazix
    "khazix": [
		Spell("khazixw",                ["khazixwmissile"],                        SFlag.SkillshotLine),
		Spell("khazixwlong",            ["khazixwmissile"],                        SFlag.SkillshotLine)
	],
    #kindred
    #kled
    #kogmaw
    #lb
    "leblanc": [
		Spell("leblancw",               [],                                        SFlag.Area),
		Spell("leblancrw",              [],                                        SFlag.Area),
		Spell("leblance",               ["leblancemissile"],                       SFlag.SkillshotLine),
		Spell("leblancre",              ["leblancremissile"],                      SFlag.SkillshotLine)
	],
    #leesin
    "leesin": [                         
		Spell("blindmonkqone",          ["blindmonkqone"],                         SFlag.SkillshotLine)
	],
    #leona
    "leona": [
		Spell("leonazenithblade",       ["leonazenithblademissile"],               SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall),
		Spell("leonasolarflare",        [],                                        SFlag.Area)
	],
    #lillia
    #lissandra
    #lucian
    "lucian": [
        Spell("lucianq",                ["lucianq"],                          SFlag.SkillshotLine, delay=0.4),
		Spell("lucianw",                ["lucianwmissile"],                          SFlag.SkillshotLine),
		Spell("lucianr",                ["lucianrmissile", "lucianrmissileoffhand"], SFlag.SkillshotLine)
	],
    #lulu
    #lux
    "lux": [                            
		Spell("luxlightbinding",        ["luxlightbindingmis"],                    SFlag.SkillshotLine),
		Spell("luxlightstrikekugel",    ["luxlightstrikekugel"],                   SFlag.Area | SFlag.CollideWindwall),
		Spell("luxmalicecannon",        ["luxmalicecannon"],                       SFlag.Line)
	],
    #malphite
    "malphite": [
		Spell("ufslash",                [],                                        SFlag.Area)
	],
    #malzahar
    #maokai
    #master yi
    #miss fortune
    "missfortune": [
		Spell("missfortunescattershot", [],                                        SFlag.Area, delay=0.25),
		Spell("missfortunebullettime",  ["missfortunebullets"],                    SFlag.Line | SFlag.CollideWindwall)
	],
    #mordekaiser
    #morgana
    "morgana": [                        
		Spell("morganaq",               ["morganaq"],                              SFlag.SkillshotLine),
		Spell("morganaw",               [],                                        SFlag.Area, delay=0.25)
	],
    #nami
    "nami": [
		Spell("namiq",                  ["namiqmissile"],                          SFlag.Area),
		Spell("namir",                  ["namirmissile"],                          SFlag.Line | SFlag.CollideWindwall)
	],
    #nasus
    "nasus": [
		Spell("nasuse",                 [],                                        SFlag.Area)
	],
    #nautilus
    #neeko
    #nidalee
    "nidalee": [
		Spell("javelintoss",            ["javelintoss"],                           SFlag.SkillshotLine),
		Spell("bushwhack",              [],                                        SFlag.Area)
	],
    #nocturne
    #nunu
    #olaf
    "olaf": [
		Spell("olafaxethrowcast",       ["olafaxethrow"],                          SFlag.Line | SFlag.CollideWindwall)
	],
    #orianna
    "orianna": [
		Spell("orianaizunacommand",     ["orianaizuna"],                           SFlag.Line | SFlag.Area | SFlag.CollideWindwall)
	],
    #ornn
    #pantheon
    "pantheon": [
		Spell("pantheonq",              ["pantheonqmissile"],                      SFlag.Line | SFlag.CollideWindwall),
		Spell("pantheonr",              ["pantheonrmissile"],                      SFlag.Area)
	],
    #poppy
    #pyke
    #qiyana
    #quinn
    #rakan
    "rakan": [
		Spell("rakanq",                ["rakanqmis"],                        SFlag.SkillshotLine),
		Spell("rakanw",                [],                                        SFlag.Area, delay=0.5)
	],
    #rammus
    #reksai
    #rell
    #renekton
    #rengar
    "rengar": [
		Spell("rengare",                ["rengaremis"],                            SFlag.SkillshotLine),
		Spell("rengareemp",             ["rengareempmis"],                         SFlag.SkillshotLine),
	],
    #riven
    #rumble
    #ryze
    "ryze": [
		Spell("ryzeq",           ["ryzeq"],                                 SFlag.SkillshotLine)
	],
    #samira
    "samira": [                        
		Spell("samiraqgun",               ["samiraqgun"],                              SFlag.SkillshotLine),
	],
    #sejuani
    #senna
    "senna": [
        Spell("sennaqcast",             ["sennaqcast"],                                SFlag.SkillshotLine),
		Spell("sennaw",                 ["sennaw"],                                SFlag.SkillshotLine),
		Spell("sennar",                 ["sennar"],                                SFlag.Line)
	],
    #seraphine
    "seraphine": [
		Spell("seraphineq", 			[], 			SFlag.Area),
		Spell("seraphineecast", 			["seraphineemissile"], 			SFlag.SkillshotLine),
		Spell("seraphiner", 			["seraphiner"], 			SFlag.SkillshotLine),
		Spell("seraphinerfow", 			[], 			SFlag.SkillshotLine),
	],
    #sett
    #shaco
    #shen
    "shen": [                           
		Spell("shene",           ["shene"], 			SFlag.Line)
	],
    #shyvana
    "shyvana": [
		Spell("shyvanafireball",        ["shyvanafireballmissile"],                SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall),
		Spell("shyvanafireballdragon2", ["shyvanafireballdragonmissile"],          SFlag.Line | SFlag.Area | SFlag.CollideChampion | SFlag.CollideWindwall)
	],
    # singed
    "singed": [
		Spell("megaadhesive",           ["singedwparticlemissile"],                SFlag.Area)
	],
    #sion
    #sivir
    "sivir": [
		Spell("sivirq",                 ["sivirqmissile"],                         SFlag.Line | SFlag.CollideWindwall)
	],
    #skarner
    #sona
    "sona": [
		Spell("sonar",                  ["sonar"],                                 SFlag.Line | SFlag.CollideWindwall)
	],
    #soraka
    "soraka": [
		Spell("sorakaq",                ["sorakaqmissile"],                        SFlag.Area),
		Spell("sorakae",                [],                                        SFlag.Area)
	],
    #swain
    "swain": [                           
		Spell("swainw",                  [],                                 SFlag.Area | SFlag.CollideWindwall),
		Spell("swaine",                  ["swaine"],                             SFlag.SkillshotLine),
		Spell("swainereturn",              ["swainereturnmissile"],         SFlag.SkillshotLine)
	],
    #sylas
    #syndra
    "syndra": [
		Spell("syndraq", 			["syndraqspell"], 			SFlag.Area),
		Spell("syndrae5", 			["syndrae5"], 			SFlag.Area),
		Spell("syndraqe", 			["syndrae"], 			SFlag.Area)
	],
    #tahm
    #taliyah
    #talon
    #taric
    "taric": [
		Spell("tarice", 			["tarice"], 			SFlag.SkillshotLine, delay=0.1)
	],
    #teemo
    #thresh
    "thresh": [
		Spell("threshq",                ["threshqmissile"],                        SFlag.SkillshotLine),
		Spell("threshw",                ["threshwlanternout"],                     SFlag.Area | SFlag.CollideWindwall)
	],
    #tristana
    #trundle
    #tryndamere
    #twistedfate
    "twistedfate": [                    
		Spell("wildcards",              ["sealfatemissile"],                       SFlag.CollideWindwall | SFlag.Line)
	],
    #twitch
    #udyr
    #urgot
    "urgot": [
		Spell("urgotq",                 ["urgotqmissile"],                         SFlag.Area | SFlag.CollideWindwall, delay = 0.2),
		Spell("urgotr",                 ["urgotr"],                                SFlag.Line | SFlag.CollideWindwall | SFlag.CollideChampion)
	],
    #varus
    "varus": [
		Spell("varusq",                 ["varusqmissile"],                         SFlag.Line | SFlag.CollideWindwall),
		Spell("varuse",                 ["varusemissile"],                         SFlag.Area),
		Spell("varusr",                 ["varusrmissile"],                         SFlag.Line | SFlag.CollideChampion | SFlag.CollideWindwall)
	],
    #vayne
    #veigar
    "veigar": [                         
		Spell("veigarbalefulstrike",    ["veigarbalefulstrikemis"],                SFlag.SkillshotLine),
		Spell("veigardarkmatter",       [],                                        SFlag.Area, delay=1.0),
		Spell("veigareventhorizon",     [],                                        SFlag.Area, delay=0.5)
	],
    #velkox
    #vi
    #viego
    "viego": [
		Spell("viegoq", 			[], 			SFlag.SkillshotLine, delay=0.4),
		Spell("viegowcast", ["viegowmis"], SFlag.SkillshotLine),
		Spell("viegorr", [], SFlag.Area)
	],
    #viktor
    #vladimir
    #volibear
    #warwick
    "warwick": [
		Spell("warwickr",               [],                                        SFlag.Area | SFlag.CollideChampion)
	],
    #wukong
    #xayah
    "xayah": [
		Spell("xayahq",                ["xayahqmissile1", "xayahqmissile2"],      SFlag.SkillshotLine)
	],
    #xerath
    "xerath": [
		Spell("xeratharcanopulse",             ["xeratharcanopulse2"],                       SFlag.SkillshotLine),
		Spell("xeratharcanebarrage2",            ["xeratharcanebarrage2"],                                        SFlag.Area),
		Spell("xerathmagespear",           ["xerathmagespearmissile"],                                        SFlag.Area),
		Spell("xerathrmissilewrapper",           ["xerathlocuspulse"],                                        SFlag.Area),
	],
    #xin
    #yasuo
    "yasuo": [
		Spell("yasuoq3",                 ["yasuoq3mis"],                             SFlag.SkillshotLine, delay=0.25)
	],
    #yone
    #yorick
    #yuumi
    #zac
    "zac": [
		Spell("zacq",                   ["zacqmissile"],                           SFlag.SkillshotLine),
		Spell("zace",                   [],                                        SFlag.Area)
	],
    #zed
    "zed": [
		Spell("zedq",       ["zedqmissile"],                          SFlag.Line)
	],
    #ziggs
    "ziggs": [                          
		Spell("ziggsq",                 ["ziggsqspell", "ziggsqspell2", "ziggsqspell3"],                              SFlag.Area | SFlag.CollideWindwall),
		Spell("ziggsw",                 ["ziggsw"],                                                                   SFlag.Area | SFlag.CollideWindwall),
		Spell("ziggse",                 ["ziggse2"],                                                                  SFlag.Area | SFlag.CollideWindwall),
		Spell("ziggsr",                 ["ziggsrboom", "ziggsrboommedium", "ziggsrboomlong", "ziggsrboomextralong"],  SFlag.Area)
	],
    #zilean
    "zilean": [
		Spell("zileanq",                ["zileanqmissile"],                        SFlag.Area | SFlag.CollideWindwall)
	],
    #zoe
    #zyra
    "zyra": [
		Spell("zyraq",                  ["zyraq"],                                        SFlag.SkillshotLine),
		Spell("zyraw",                  [],                                        SFlag.Area),
		Spell("zyrae",                  ["zyrae"],                                 SFlag.SkillshotLine),
		Spell("zyrar",                  [],                                         SFlag.Line | SFlag.Area | SFlag.CollideChampion | SFlag.CollideWindwall),
		Spell("zyrapassivedeathmanager",                  ["zyrapassivedeathmanager"],                                        SFlag.SkillshotLine)
	],
}

def draw_prediction_info(game, ui):
	global ChampionSpells, Version
	
	ui.separator()
	ui.text("Using LPrediction " + Version, Color.PURPLE)
	if is_champ_supported(game.player):
		ui.text(game.player.name.upper() + " has skillshot prediction support", Color.GREEN)
	else:
		ui.text(game.player.name.upper() + " doesnt have skillshot prediction support", Color.RED)
	
	if ui.treenode(f'Supported Champions ({len(ChampionSpells)})'):
		for champ, spells in sorted(ChampionSpells.items()):
			ui.text(f"{champ.upper()} {' '*(20 - len(champ))}: {str([spell.name for spell in spells])}")
			
		ui.treepop()

def get_skillshot_range(game, skill_name):
	global Spells
	if skill_name not in Spells:
		raise Exception("Not a skillshot")
	
	# Get the range of the missile if it has a missile
	skillshot = Spells[skill_name]
	if len(skillshot.missiles) > 0:
		return game.get_spell_info(skillshot.missiles[0]).cast_range
		
	# If it doesnt have a missile get simply the cast_range from the skill
	info = game.get_spell_info(skill_name)
	return info.cast_range*2.0 if is_skillshot_cone(skill_name) else info.cast_range

def is_skillshot(skill_name):
	global Spells, MissileToSpell
	return skill_name in Spells or skill_name in MissileToSpell
	
def get_missile_parent_spell(missile_name):
	global MissileToSpell
	return MissileToSpell.get(missile_name, None)
	
def is_champ_supported(champ):
	global ChampionSpells
	return champ.name in ChampionSpells
	
def is_skillshot_cone(skill_name):
	if skill_name not in Spells:
		return False
	return Spells[skill_name].flags & SFlag.Cone
	
def is_last_hitable(game, player, enemy):
	missile_speed = player.basic_missile_speed + 1
		
	hit_dmg = items.get_onhit_physical(player, enemy) + items.get_onhit_magical(player, enemy)
	
	hp = enemy.health
	atk_speed = player.base_atk_speed * player.atk_speed_multi
	t_until_basic_hits = game.distance(player, enemy)/missile_speed#(missile_speed*atk_speed/player.base_atk_speed)

	for missile in game.missiles:
		if missile.dest_id == enemy.id:
			src = game.get_obj_by_id(missile.src_id)
			if src:
				t_until_missile_hits = game.distance(missile, enemy)/(missile.speed + 1)
			
				if t_until_missile_hits < t_until_basic_hits:
					hp -= src.base_atk

	return hp - hit_dmg <= 0
	
# Returns a point where the mouse should click to cast a spells taking into account the targets movement speed
def castpoint_for_collision(game, spell, caster, target):
	global Spells

	print('predicted')
	if spell.name not in Spells:
		return None
	
	# Get extra data for spell that isnt provided by lview
	spell_extra = Spells[spell.name]
	if len(spell_extra.missiles) > 0:
		missile = game.get_spell_info(spell_extra.missiles[0])
	else:
		missile = spell
		
	t_delay = spell.delay + spell_extra.delay
	if missile.travel_time > 0.0:
		t_missile = missile.travel_time
	else:
		t_missile = (missile.cast_range / missile.speed) if len(spell_extra.missiles) > 0 and missile.speed > 0.0 else 0.0
			
	# Get direction of target
	target_dir = target.pos.sub(target.prev_pos).normalize()
	if math.isnan(target_dir.x):
		target_dir.x = 0.0
	if math.isnan(target_dir.y):
		target_dir.y = 0.0
	if math.isnan(target_dir.z):
		target_dir.z = 0.0
	#print(f'{target_dir.x} {target_dir.y} {target_dir.z}')

	# If the spell is a line we simulate the main missile to get the collision point
	if spell_extra.flags & SFlag.Line:
		
		iterations = int(missile.cast_range/30.0)
		step = t_missile/iterations
		
		last_dist = 9999999
		last_target_pos = None
		for i in range(iterations):
			t = i*step
			target_future_pos = target.pos.add(target_dir.scale((t_delay + t)*target.movement_speed))
			spell_dir = target_future_pos.sub(caster.pos).normalize().scale(t*missile.speed)
			spell_future_pos = caster.pos.add(spell_dir)
			
			dist = target_future_pos.distance(spell_future_pos)
			#print(dist)
			if dist < missile.width/2.0:
				return target_future_pos
			elif dist > last_dist:
				return last_target_pos
			else:
				last_dist = dist
				last_target_pos = target_future_pos
				
		return None
		
	# If the spell is an area spell we return the position of the player when the spell procs
	elif spell_extra.flags & SFlag.Area:
		return target.pos.add(target_dir.scale((t_delay + t_missile)*target.movement_speed))
	else:
		return target.pos