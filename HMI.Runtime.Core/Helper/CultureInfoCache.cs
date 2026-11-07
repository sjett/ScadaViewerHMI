// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Globalization;
namespace HMI.Runtime.Core.Helper
{
	public static class CultureInfoCache
	{
		private static readonly Dictionary<int, CultureInfo> _culturesByLanguageId = new Dictionary<int, CultureInfo>();
		public static CultureInfo GetCultureInfo(int languageID)
		{
			if (!_culturesByLanguageId.ContainsKey(languageID))
			{
                _culturesByLanguageId.Add(languageID, CreateCultureInfo(languageID));
			}
			return _culturesByLanguageId[languageID];
		}
		private static CultureInfo CreateCultureInfo(int languageID)
		{
			CultureInfo result;
			try
			{
				if (languageID <= 8193)
				{
					if (languageID <= 3073)
					{
						if (languageID <= 2070)
						{
							if (languageID <= 1125)
							{
								switch (languageID)
								{
								case 1:
									result = new CultureInfo("ar");
									return result;
								case 2:
									result = new CultureInfo("bg");
									return result;
								case 3:
									result = new CultureInfo("ca");
									return result;
								case 4:
									result = new CultureInfo("zh-Hans");
									return result;
								case 5:
									result = new CultureInfo("cs");
									return result;
								case 6:
									result = new CultureInfo("da");
									return result;
								case 7:
									result = new CultureInfo("de");
									return result;
								case 8:
									result = new CultureInfo("el");
									return result;
								case 9:
									result = new CultureInfo("en");
									return result;
								case 10:
									result = new CultureInfo("es");
									return result;
								case 11:
									result = new CultureInfo("fi");
									return result;
								case 12:
									result = new CultureInfo("fr");
									return result;
								case 13:
									result = new CultureInfo("he");
									return result;
								case 14:
									result = new CultureInfo("hu");
									return result;
								case 15:
									result = new CultureInfo("is");
									return result;
								case 16:
									result = new CultureInfo("it");
									return result;
								case 17:
									result = new CultureInfo("ja");
									return result;
								case 18:
									result = new CultureInfo("ko");
									return result;
								case 19:
									result = new CultureInfo("nl");
									return result;
								case 20:
									result = new CultureInfo("no");
									return result;
								case 21:
									result = new CultureInfo("pl");
									return result;
								case 22:
									result = new CultureInfo("pt");
									return result;
								case 23:
								case 40:
								case 46:
								case 48:
								case 49:
								case 50:
								case 51:
								case 52:
								case 53:
								case 58:
								case 59:
								case 60:
								case 61:
								case 66:
								case 69:
								case 72:
								case 76:
								case 77:
								case 81:
								case 82:
								case 83:
								case 84:
								case 85:
								case 88:
								case 89:
								case 91:
								case 92:
								case 93:
								case 94:
								case 95:
								case 96:
								case 97:
								case 98:
								case 99:
								case 100:
									break;
								case 24:
									result = new CultureInfo("ro");
									return result;
								case 25:
									result = new CultureInfo("ru");
									return result;
								case 26:
									result = new CultureInfo("hr");
									return result;
								case 27:
									result = new CultureInfo("sk");
									return result;
								case 28:
									result = new CultureInfo("sq");
									return result;
								case 29:
									result = new CultureInfo("sv");
									return result;
								case 30:
									result = new CultureInfo("th");
									return result;
								case 31:
									result = new CultureInfo("tr");
									return result;
								case 32:
									result = new CultureInfo("ur");
									return result;
								case 33:
									result = new CultureInfo("id");
									return result;
								case 34:
									result = new CultureInfo("uk");
									return result;
								case 35:
									result = new CultureInfo("be");
									return result;
								case 36:
									result = new CultureInfo("sl");
									return result;
								case 37:
									result = new CultureInfo("et");
									return result;
								case 38:
									result = new CultureInfo("lv");
									return result;
								case 39:
									result = new CultureInfo("lt");
									return result;
								case 41:
									result = new CultureInfo("fa");
									return result;
								case 42:
									result = new CultureInfo("vi");
									return result;
								case 43:
									result = new CultureInfo("hy");
									return result;
								case 44:
									result = new CultureInfo("az");
									return result;
								case 45:
									result = new CultureInfo("eu");
									return result;
								case 47:
									result = new CultureInfo("mk");
									return result;
								case 54:
									result = new CultureInfo("af");
									return result;
								case 55:
									result = new CultureInfo("ka");
									return result;
								case 56:
									result = new CultureInfo("fo");
									return result;
								case 57:
									result = new CultureInfo("hi");
									return result;
								case 62:
									result = new CultureInfo("ms");
									return result;
								case 63:
									result = new CultureInfo("kk");
									return result;
								case 64:
									result = new CultureInfo("ky");
									return result;
								case 65:
									result = new CultureInfo("sw");
									return result;
								case 67:
									result = new CultureInfo("uz");
									return result;
								case 68:
									result = new CultureInfo("tt");
									return result;
								case 70:
									result = new CultureInfo("pa");
									return result;
								case 71:
									result = new CultureInfo("gu");
									return result;
								case 73:
									result = new CultureInfo("ta");
									return result;
								case 74:
									result = new CultureInfo("te");
									return result;
								case 75:
									result = new CultureInfo("kn");
									return result;
								case 78:
									result = new CultureInfo("mr");
									return result;
								case 79:
									result = new CultureInfo("sa");
									return result;
								case 80:
									result = new CultureInfo("mn");
									return result;
								case 86:
									result = new CultureInfo("gl");
									return result;
								case 87:
									result = new CultureInfo("kok");
									return result;
								case 90:
									result = new CultureInfo("syr");
									return result;
								case 101:
									result = new CultureInfo("dv");
									return result;
								default:
									switch (languageID)
									{
									case 1025:
										result = new CultureInfo("ar-SA");
										return result;
									case 1026:
										result = new CultureInfo("bg-BG");
										return result;
									case 1027:
										result = new CultureInfo("ca-ES");
										return result;
									case 1028:
										result = new CultureInfo("zh-TW");
										return result;
									case 1030:
										result = new CultureInfo("da-DK");
										return result;
									case 1031:
										result = new CultureInfo("de-DE");
										return result;
									case 1032:
										result = new CultureInfo("el-GR");
										return result;
									case 1033:
										result = new CultureInfo("en-US");
										return result;
									case 1034:
										result = new CultureInfo("es-ES_tradnl");
										return result;
									case 1035:
										result = new CultureInfo("fi-FI");
										return result;
									case 1036:
										result = new CultureInfo("fr-FR");
										return result;
									case 1037:
										result = new CultureInfo("he-IL");
										return result;
									case 1038:
										result = new CultureInfo("hu-HU");
										return result;
									case 1039:
										result = new CultureInfo("is-IS");
										return result;
									case 1040:
										result = new CultureInfo("it-IT");
										return result;
									case 1041:
										result = new CultureInfo("ja-JP");
										return result;
									case 1042:
										result = new CultureInfo("ko-KR");
										return result;
									case 1043:
										result = new CultureInfo("nl-NL");
										return result;
									case 1044:
										result = new CultureInfo("nb-NO");
										return result;
									case 1045:
										result = new CultureInfo("pl-PL");
										return result;
									case 1046:
										result = new CultureInfo("pt-BR");
										return result;
									case 1048:
										result = new CultureInfo("ro-RO");
										return result;
									case 1049:
										result = new CultureInfo("ru-RU");
										return result;
									case 1050:
										result = new CultureInfo("hr-HR");
										return result;
									case 1051:
										result = new CultureInfo("sk-SK");
										return result;
									case 1052:
										result = new CultureInfo("sq-AL");
										return result;
									case 1053:
										result = new CultureInfo("sv-SE");
										return result;
									case 1054:
										result = new CultureInfo("th-TH");
										return result;
									case 1055:
										result = new CultureInfo("tr-TR");
										return result;
									case 1056:
										result = new CultureInfo("ur-PK");
										return result;
									case 1057:
										result = new CultureInfo("id-ID");
										return result;
									case 1058:
										result = new CultureInfo("uk-UA");
										return result;
									case 1059:
										result = new CultureInfo("be-BY");
										return result;
									case 1060:
										result = new CultureInfo("sl-SI");
										return result;
									case 1061:
										result = new CultureInfo("et-EE");
										return result;
									case 1062:
										result = new CultureInfo("lv-LV");
										return result;
									case 1063:
										result = new CultureInfo("lt-LT");
										return result;
									case 1065:
										result = new CultureInfo("fa-IR");
										return result;
									case 1066:
										result = new CultureInfo("vi-VN");
										return result;
									case 1067:
										result = new CultureInfo("hy-AM");
										return result;
									case 1068:
										result = new CultureInfo("az-Latn-AZ");
										return result;
									case 1069:
										result = new CultureInfo("eu-ES");
										return result;
									case 1071:
										result = new CultureInfo("mk-MK");
										return result;
									case 1078:
										result = new CultureInfo("af-ZA");
										return result;
									case 1079:
										result = new CultureInfo("ka-GE");
										return result;
									case 1080:
										result = new CultureInfo("fo-FO");
										return result;
									case 1081:
										result = new CultureInfo("hi-IN");
										return result;
									case 1086:
										result = new CultureInfo("ms-MY");
										return result;
									case 1087:
										result = new CultureInfo("kk-KZ");
										return result;
									case 1088:
										result = new CultureInfo("ky-KG");
										return result;
									case 1089:
										result = new CultureInfo("sw-KE");
										return result;
									case 1091:
										result = new CultureInfo("uz-Latn-UZ");
										return result;
									case 1092:
										result = new CultureInfo("tt-RU");
										return result;
									case 1094:
										result = new CultureInfo("pa-IN");
										return result;
									case 1095:
										result = new CultureInfo("gu-IN");
										return result;
									case 1097:
										result = new CultureInfo("ta-IN");
										return result;
									case 1098:
										result = new CultureInfo("te-IN");
										return result;
									case 1099:
										result = new CultureInfo("kn-IN");
										return result;
									case 1102:
										result = new CultureInfo("mr-IN");
										return result;
									case 1103:
										result = new CultureInfo("sa-IN");
										return result;
									case 1104:
										result = new CultureInfo("mn-MN");
										return result;
									case 1110:
										result = new CultureInfo("gl-ES");
										return result;
									case 1111:
										result = new CultureInfo("kok-IN");
										return result;
									case 1114:
										result = new CultureInfo("syr-SY");
										return result;
									case 1125:
										result = new CultureInfo("dv-MV");
										return result;
									}
									break;
								}
							}
							else
							{
								if (languageID == 2049)
								{
									result = new CultureInfo("ar-IQ");
									return result;
								}
								switch (languageID)
								{
								case 2052:
									result = new CultureInfo("zh-CN");
									return result;
								case 2053:
								case 2054:
								case 2056:
								case 2059:
									break;
								case 2055:
									result = new CultureInfo("de-CH");
									return result;
								case 2057:
									result = new CultureInfo("en-GB");
									return result;
								case 2058:
									result = new CultureInfo("es-MX");
									return result;
								case 2060:
									result = new CultureInfo("fr-BE");
									return result;
								default:
									switch (languageID)
									{
									case 2064:
										result = new CultureInfo("it-CH");
										return result;
									case 2067:
										result = new CultureInfo("nl-BE");
										return result;
									case 2068:
										result = new CultureInfo("nn-NO");
										return result;
									case 2070:
										result = new CultureInfo("pt-PT");
										return result;
									}
									break;
								}
							}
						}
						else
						{
							if (languageID <= 2092)
							{
								if (languageID == 2074)
								{
									result = new CultureInfo("sr-Latn-CS");
									return result;
								}
								if (languageID == 2077)
								{
									result = new CultureInfo("sv-FI");
									return result;
								}
								if (languageID == 2092)
								{
									result = new CultureInfo("az-Cyrl-AZ");
									return result;
								}
							}
							else
							{
								if (languageID == 2110)
								{
									result = new CultureInfo("ms-BN");
									return result;
								}
								if (languageID == 2115)
								{
									result = new CultureInfo("uz-Cyrl-UZ");
									return result;
								}
								if (languageID == 3073)
								{
									result = new CultureInfo("ar-EG");
									return result;
								}
							}
						}
					}
					else
					{
						if (languageID <= 5121)
						{
							if (languageID <= 3098)
							{
								switch (languageID)
								{
								case 3076:
									result = new CultureInfo("zh-HK");
									return result;
								case 3077:
								case 3078:
								case 3080:
								case 3083:
									break;
								case 3079:
									result = new CultureInfo("de-AT");
									return result;
								case 3081:
									result = new CultureInfo("en-AU");
									return result;
								case 3082:
									result = new CultureInfo("es-ES");
									return result;
								case 3084:
									result = new CultureInfo("fr-CA");
									return result;
								default:
									if (languageID == 3098)
									{
										result = new CultureInfo("sr-Cyrl-CS");
										return result;
									}
									break;
								}
							}
							else
							{
								if (languageID == 4097)
								{
									result = new CultureInfo("ar-LY");
									return result;
								}
								switch (languageID)
								{
								case 4100:
									result = new CultureInfo("zh-SG");
									return result;
								case 4101:
								case 4102:
								case 4104:
								case 4107:
									break;
								case 4103:
									result = new CultureInfo("de-LU");
									return result;
								case 4105:
									result = new CultureInfo("en-CA");
									return result;
								case 4106:
									result = new CultureInfo("es-GT");
									return result;
								case 4108:
									result = new CultureInfo("fr-CH");
									return result;
								default:
									if (languageID == 5121)
									{
										result = new CultureInfo("ar-DZ");
										return result;
									}
									break;
								}
							}
						}
						else
						{
							if (languageID <= 6156)
							{
								switch (languageID)
								{
								case 5124:
									result = new CultureInfo("zh-MO");
									return result;
								case 5125:
								case 5126:
								case 5128:
								case 5131:
									break;
								case 5127:
									result = new CultureInfo("de-LI");
									return result;
								case 5129:
									result = new CultureInfo("en-NZ");
									return result;
								case 5130:
									result = new CultureInfo("es-CR");
									return result;
								case 5132:
									result = new CultureInfo("fr-LU");
									return result;
								default:
									if (languageID == 6145)
									{
										result = new CultureInfo("ar-MA");
										return result;
									}
									switch (languageID)
									{
									case 6153:
										result = new CultureInfo("en-IE");
										return result;
									case 6154:
										result = new CultureInfo("es-PA");
										return result;
									case 6156:
										result = new CultureInfo("fr-MC");
										return result;
									}
									break;
								}
							}
							else
							{
								if (languageID == 7169)
								{
									result = new CultureInfo("ar-TN");
									return result;
								}
								switch (languageID)
								{
								case 7177:
									result = new CultureInfo("en-ZA");
									return result;
								case 7178:
									result = new CultureInfo("es-DO");
									return result;
								default:
									if (languageID == 8193)
									{
										result = new CultureInfo("ar-OM");
										return result;
									}
									break;
								}
							}
						}
					}
				}
				else
				{
					if (languageID <= 13322)
					{
						if (languageID <= 10250)
						{
							if (languageID <= 9217)
							{
								switch (languageID)
								{
								case 8201:
									result = new CultureInfo("en-JM");
									return result;
								case 8202:
									result = new CultureInfo("es-VE");
									return result;
								default:
									if (languageID == 9217)
									{
										result = new CultureInfo("ar-YE");
										return result;
									}
									break;
								}
							}
							else
							{
								switch (languageID)
								{
								case 9225:
									result = new CultureInfo("en-029");
									return result;
								case 9226:
									result = new CultureInfo("es-CO");
									return result;
								default:
									if (languageID == 10241)
									{
										result = new CultureInfo("ar-SY");
										return result;
									}
									switch (languageID)
									{
									case 10249:
										result = new CultureInfo("en-BZ");
										return result;
									case 10250:
										result = new CultureInfo("es-PE");
										return result;
									}
									break;
								}
							}
						}
						else
						{
							if (languageID <= 12289)
							{
								if (languageID == 11265)
								{
									result = new CultureInfo("ar-JO");
									return result;
								}
								switch (languageID)
								{
								case 11273:
									result = new CultureInfo("en-TT");
									return result;
								case 11274:
									result = new CultureInfo("es-AR");
									return result;
								default:
									if (languageID == 12289)
									{
										result = new CultureInfo("ar-LB");
										return result;
									}
									break;
								}
							}
							else
							{
								switch (languageID)
								{
								case 12297:
									result = new CultureInfo("en-ZW");
									return result;
								case 12298:
									result = new CultureInfo("es-EC");
									return result;
								default:
									if (languageID == 13313)
									{
										result = new CultureInfo("ar-KW");
										return result;
									}
									switch (languageID)
									{
									case 13321:
										result = new CultureInfo("en-PH");
										return result;
									case 13322:
										result = new CultureInfo("es-CL");
										return result;
									}
									break;
								}
							}
						}
					}
					else
					{
						if (languageID <= 16385)
						{
							if (languageID <= 14346)
							{
								if (languageID == 14337)
								{
									result = new CultureInfo("ar-AE");
									return result;
								}
								if (languageID == 14346)
								{
									result = new CultureInfo("es-UY");
									return result;
								}
							}
							else
							{
								if (languageID == 15361)
								{
									result = new CultureInfo("ar-BH");
									return result;
								}
								if (languageID == 15370)
								{
									result = new CultureInfo("es-PY");
									return result;
								}
								if (languageID == 16385)
								{
									result = new CultureInfo("ar-QA");
									return result;
								}
							}
						}
						else
						{
							if (languageID <= 18442)
							{
								if (languageID == 16394)
								{
									result = new CultureInfo("es-BO");
									return result;
								}
								if (languageID == 17418)
								{
									result = new CultureInfo("es-SV");
									return result;
								}
								if (languageID == 18442)
								{
									result = new CultureInfo("es-HN");
									return result;
								}
							}
							else
							{
								if (languageID == 19466)
								{
									result = new CultureInfo("es-NI");
									return result;
								}
								if (languageID == 20490)
								{
									result = new CultureInfo("es-PR");
									return result;
								}
								if (languageID == 31748)
								{
									result = new CultureInfo("zh-Hant");
									return result;
								}
							}
						}
					}
				}
				result = CultureInfo.CurrentCulture;
			}
			catch
			{
				result = CultureInfo.CurrentCulture;
			}
			return result;
		}
	}
}
