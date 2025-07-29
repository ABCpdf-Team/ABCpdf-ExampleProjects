// ===========================================================================
//	©2013-2024 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace WebSupergoo.ABCpdf13.Drawing.Text {
	#region BuiltInFontMetrics
	internal sealed class BuiltInFontMetrics : FontMetrics {
		#region AdobeFromUnicode
		internal static readonly int[] AdobeFromUnicode = {
															  0,     1,     2,     3,     4,     5,     6,     7,     8,     9, //     0
															  10,   11,   12,   13,   14,   15,   16,   17,   18,   19, //   10
															  20,   21,   22,   23,   24,   25,   26,   27,   28,   29, //   20
															  30,   31,   32,   33,   34,   35,   36,   37,   38,   39, //   30
															  40,   41,   42,   43,   44,   45,   46,   47,   48,   49, //   40
															  50,   51,   52,   53,   54,   55,   56,   57,   58,   59, //   50
															  60,   61,   62,   63,   64,   65,   66,   67,   68,   69, //   60
															  70,   71,   72,   73,   74,   75,   76,   77,   78,   79, //   70
															  80,   81,   82,   83,   84,   85,   86,   87,   88,   89, //   80
															  90,   91,   92,   93,   94,   95,   96,   97,   98,   99, //   90
															  100, 101, 102, 103, 104, 105, 106, 107, 108, 109, // 100
															  110, 111, 112, 113, 114, 115, 116, 117, 118, 119, // 110
															  120, 121, 122, 123, 124, 125, 126, 127, 128, 129, // 120
															  130, 131, 132, 133, 134, 135, 136, 137, 138, 139, // 130
															  140, 141, 142, 143, 144, 145, 146, 147, 148, 149, // 140
															  150, 151, 152, 153, 154, 155, 156, 157, 158, 159, // 150
															  160, 161, 162, 163, 164, 165, 166, 167, 168, 169, // 160
															  170, 171, 172,   45, 174, 175, 176, 177, 178, 179, // 170 - 173/-
															  180, 181, 182, 183, 184, 185, 186, 187, 188, 189, // 180
															  190, 191, 192, 193, 194, 195, 196, 197, 198, 199, // 190
															  200, 201, 202, 203, 204, 205, 206, 207, 208, 209, // 200
															  210, 211, 212, 213, 214, 215, 216, 217, 218, 219, // 210
															  220, 221, 222, 223, 224, 225, 226, 227, 228, 229, // 220
															  230, 231, 232, 233, 234, 235, 236, 237, 238, 239, // 230
															  240, 241, 242, 243, 244, 245, 246, 247, 248, 249, // 240
															  250, 251, 252, 253, 254, 255,   88,   88,   88,   88, // 250
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 260
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 270
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 280
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 290
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 300
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 310
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 320
															  88,   88,   88,   88,   88,   88,   88,   88, 140, 156, // 330 - 338/OE, 339/oe
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 340
															  88,   88, 138,   88,   88,   88,   88,   88,   88,   88, // 350 - 352/S caron
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 360
															  88,   88,   88,   88,   88,   88, 159,   88,   88,   88, // 370 - 376/Y dieresis
															  88, 142, 158,   88,   88,   88,   88,   88,   88,   88, // 380 - 381/Z caron, 382/z caron
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 390
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 400
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 410
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 420
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 430
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 440
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 450
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 460
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 470
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 480
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 490
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 500
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 510
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 520
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 530
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 540
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 550
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 560
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 570
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 580
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 590
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 600
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 610
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 620
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 630
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 640
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 650
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 660
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 670
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 680
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 690
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 700
															  136,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 710 - 710/circumflex
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 720
															  225,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 730 - 730/breve
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 740
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 750
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 760
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 770
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 780
															  88,   88,   88,   88,   88,   88,   88,   88,   88,   88, // 790
															  88 };
		#endregion

		internal static readonly char[] DelimiterToken = { ' ', '\t' };
		internal static readonly char[] DelimiterSemicolon = { ';' };

		internal BuiltInFontMetrics(Stream stream) {
			using (StreamReader streamReader = new StreamReader(stream)) {
				string line = null;
				while ((line = streamReader.ReadLine()) != null) {
					if (line.StartsWith("Comment")) 
						continue;

					string[] token = line.Split(DelimiterToken, 5);
					if (token.Length == 0) 
						continue;

					switch (token[0]) {
						case "StartFontMetrics":
							FontMetricsVersion = token[1];
							break;
						case "MetricsSets":
							MetricsSets = int.Parse(token[1]);
							break;
						case "FontName":
							FontName = token[1];
							break;
						case "FullName":
							FullName = token[1];
							break;
						case "FamilyName":
							FamilyName = token[1];
							break;
						case "Weight":
							Weight = token[1];
							break;
						case "FontBBox":
							FontBBox_llx  = double.Parse(token[1]);
							FontBBox_lly = double.Parse(token[2]);
							FontBBox_urx = double.Parse(token[3]);
							FontBBox_ury = double.Parse(token[4]);
							break;
						case "Version":
							Version = token[1];
							break;
						case "Notice":
							Notice = line.Substring(7);
							break;
						case "EncodingScheme":
							EncodingScheme = token[1];
							break;
						case "CharacterSet":
							CharacterSet = token[1];
							break;
						case "IsBaseFont":
							IsBaseFont = bool.Parse(token[1]);
							break;
						case "CapHeight":
							CapHeight = double.Parse(token[1]);
							break;
						case "XHeight":
							XHeight = double.Parse(token[1]);
							break;
						case "Ascender":
							Ascender = double.Parse(token[1]);
							break;
						case "Descender":
							Descender = double.Parse(token[1]);
							break;
						case "StdHW":
							StdHW = double.Parse(token[1]);
							break;
						case "StdVW":
							StdVW = double.Parse(token[1]);
							break;
						case "UnderlinePosition":
							UnderlinePosition = double.Parse(token[1]);
							break;
						case "UnderlineThickness":
							UnderlineThickness = double.Parse(token[1]);
							break;
						case "ItalicAngle":
							ItalicAngle = double.Parse(token[1]);
							break;
						case "IsFixedPitch":
							IsFixedPitch = bool.Parse(token[1]);
							break;
						case "StartCharMetrics":
							CharMetricsCount = int.Parse(token[1]);
							ParseCharMetrics(streamReader);
							break;
						default:
							break;
					}
				}
			}
		}

		internal override System.Drawing.SizeF MeasureString(string s, double size, TextState state) {
			if (s.Length == 0) 
				return new System.Drawing.SizeF(0, 0);

			double width = 0;
			
			foreach (char c in s) {
				short ch = (short)c;
				CharMetrics cm = GetCharMetrics(ch);
				if (cm != null) 
					width += cm.WidthX;
				if (c == ' ') 
					width += state.WordSpacing;
			}
			
			width += (s.Length - 1) * state.CharacterSpacing;

			double capHeight = CapHeight;
			if (double.IsNaN(capHeight)) 
				capHeight = FontBBox_ury - FontBBox_lly;
			
			return new System.Drawing.SizeF(
				(float)(GetSizeFactor(size) * width / 1000),
				(float)(GetSizeFactor(size) * CapHeight / 1000));
		}

		internal CharMetrics GetCharMetrics(int ch) {
			if (ch >= CharMetrics.Length) {
				if (ch == 8364) 
					return CharMetrics[128];
				else
					return null;
			}
			return CharMetrics[ch];
		}

		internal override double GetSizeFactor(double size) {
			if (FontName == "Symbol" || FontName == "ZapfDingbats") {
				CharMetrics cm  = GetCharMetrics(74);
				return size * 1000 / (cm.BBox_ury - cm.BBox_lly);
			}
			else {
				return size * 1000 / CapHeight;
			}
		}


		internal void ParseCharMetrics(StreamReader reader) {
			if (CharMetricsCount > 0) {
				CharMetrics = new CharMetrics[BuiltInFontMetrics.AdobeFromUnicode.Length];

				string line = null;
				while ((line = reader.ReadLine()) != null) {
					if (line.StartsWith("EndCharMetrics"))
						return;
					new BuiltInCharMetrics(line, this);
				}
			}

			CharMetrics[173] = CharMetrics[45];
		}
	}
	#endregion

	#region BuiltInCharMetrics 
	internal class BuiltInCharMetrics : CharMetrics {
		internal BuiltInCharMetrics(string line, FontMetrics afm) {
			string[] lineToken = line.Split(BuiltInFontMetrics.DelimiterSemicolon, 10);

			for (int expr = 0; expr < lineToken.Length; expr++) {
				if (lineToken[expr].Length == 0) 
					continue;
				
				string[] token = lineToken[expr].Trim().Split(BuiltInFontMetrics.DelimiterToken, 5);

				switch (token[0]) {
					case "C":
						CharacterCode = int.Parse(token[1], NumberFormatInfo.InvariantInfo);
						break;
					case "CH":
						CharacterCode = int.Parse(token[1], NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
						break;
					case "WX":
					case "W0X":
						WidthX = double.Parse(token[1], NumberFormatInfo.InvariantInfo);
						break;
					case "N":
						Name = token[1];
						break;
					case "B":
						BBox_llx = double.Parse(token[1]);
						BBox_lly = double.Parse(token[2]);
						BBox_urx = double.Parse(token[3]);
						BBox_ury = double.Parse(token[4]);
						break;
					case "L":
						if (Ligatures == null) 
							Ligatures = new List<Ligature>(10);
						Ligatures.Add(new Ligature(token[1], token[2]));
						break;
					default:
						break;
				}
			}

			int index = CharacterCode;
			if (afm.FontName != "Symbol" && afm.FontName != "ZapfDingbats") {
				switch (index) {
					case  39:  // quoteright
						index =   0;
						break;
					case  96:   // quoteleft 
						index =   0;
						break;
					case 127:
					case 128:
					case 129:
					case 130:
					case 131:
					case 132:
					case 133:
					case 134:
					case 135:
					case 136:
					case 137:
					case 138:
					case 139:
					case 140:
					case 141:
					case 142:
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 151:
					case 152:
					case 153:
					case 154:
					case 155:
					case 156:
					case 157:
					case 158:
					case 159:
					case 160:
						index =  -1;
						break;
					case 164:
						index =   0;
						break;  // fraction "/"
					case 166:
						index =   0;
						break;  // florin
					case 168:
						index = 164;
						break;  // currency "¤"
					case 169:
						index =  39;
						break;  // quotesingle "'"
					case 170:
						index =   0;
						break;  // quotedblleft
					case 172:
						index =   0;
						break;  // guilsinglleft
					case 173:
						index =   0;
						break;  // guilsinglright
					case 174:
						index =   0;  break;  // fi
					case 175:
						index =   0;
						break;  // fl
					case 176:
						index =  -1;
						break;  // does not exist!
					case 177:
						index =   0;
						break;  // endash "-"
					case 178:
						index =   0;
						break;  // dagger
					case 179:
						index =   0;
						break;  // daggerdbl
					case 180:
						index = 183;
						break;  // periodcentered
					case 183:
						index =   0;
						break;  // bullet
					case 184:
						index =   0;
						break;  // quotesinglbase
					case 185:
						index =   0;
						break;  // quotedblbase
					case 186:
						index =   0;
						break;  // quotedblright
					case 188:
						index =   0;
						break;  // ellipsis
					case 189:
						index =   0;
						break;  // perthousand
					case 190:
					case 192:
						index =  -1;
						break;
					case 193:
						index =  96;
						break;  // grave "`"
					case 194:
						index = 180;
						break;  // acute "´"
					case 195:
						index =   0;
						break;  // circumflex "^"
					case 196:
						index =   0;
						break;  // tilde "~"
					case 197:
						index = 175;
						break;  // macron
					case 198:
						index =   0;
						break;  // breve
					case 199:
						index =   0;
						break;  // dotaccent
					case 200:
						index = 168;
						break;  // dieresis "¨"
					case 201:
						index =  -1;
						break;  // does not exist!
					case 202:
						index =   0;
						break;  // ring
					case 203:
						index = 184;
						break;  // cedilla "¨"
					case 204:
						index =  -1;
						break;  // does not exist!
					case 205:
						index =   0;
						break;  // hungarumlaut
					case 206:
						index =   0;
						break;  // ogonek
					case 207:
						index =   0;
						break;  // caron
					case 208:
						index =   0;
						break;  // emdash
					case 209:
					case 210:
					case 211:
					case 212:
					case 213:
					case 214:
					case 215:
					case 216:
					case 217:
					case 218:
					case 219:
					case 220:
					case 221:
					case 222:
					case 223:
					case 224:
						index =  -1;
						break;
					case 225:
						index = 198;
						break;  // AE
					case 226:
						index =  -1;
						break;
					case 227:
						index = 170;
						break;  // ordfeminine
					case 228:
					case 229:
					case 230:
					case 231:
						index =  -1;
						break;
					case 232:
						index =   0;
						break;  // Lslash
					case 233:
						index = 216;
						break;  // Oslash
					case 234:
						index =   0;
						break;  // OE
					case 235:
						index = 186;
						break;  // ordmasculine
					case 236:
					case 237:
					case 238:
					case 239:
					case 240:
						index =  -1;
						break;
					case 241:
						index = 230;
						break;  // ae
					case 242:
						index =  -1;
						break;  // does not exist!
					case 243:
					case 244:
						index =  -1;
						break;
					case 245:
						index =   0;
						break;  // dotlessi
					case 246:
					case 247:
						index =  -1;
						break;
					case 248:
						index =   0;
						break;  // lslash
					case 249:
						index = 248;
						break;  // oslash
					case 250:
						index =   0;
						break;  // oe
					case 251:
						index = 223;
						break;  // germandbls
					case -1: {
						switch (Name) {
							case "brokenbar":      index = 166;  break;
							case "copyright":      index = 169;  break;
							case "logicalnot":     index = 172;  break;
							case "registered":     index = 174;  break;
							case "degree":         index = 176;  break;
							case "plusminus":      index = 177;  break;
							case "twosuperior":    index = 178;  break;
							case "threesuperior":  index = 179;  break;
							case "mu":             index = 181;  break;
							case "onesuperior":    index = 185;  break;
							case "onequarter":     index = 188;  break;
							case "onehalf":        index = 189;  break;
							case "threequarters":  index = 190;  break;
							case "Agrave":         index = 192;  break;
							case "Aacute":         index = 193;  break;
							case "Acircumflex":    index = 194;  break;
							case "Atilde":         index = 195;  break;
							case "Adieresis":      index = 196;  break;
							case "Aring":          index = 197;  break;
							case "Ccedilla":       index = 199;  break;
							case "Egrave":         index = 200;  break;
							case "Eacute":         index = 201;  break;
							case "Ecircumflex":    index = 202;  break;
							case "Edieresis":      index = 203;  break;
							case "Igrave":         index = 204;  break;
							case "Iacute":         index = 205;  break;
							case "Icircumflex":    index = 206;  break;
							case "Idieresis":      index = 207;  break;
							case "Eth":            index = 208;  break;
							case "Ntilde":         index = 209;  break;
							case "Ograve":         index = 210;  break;
							case "Oacute":         index = 211;  break;
							case "Ocircumflex":    index = 212;  break;
							case "Otilde":         index = 213;  break;
							case "Odieresis":      index = 214;  break;
							case "multiply":       index = 215;  break;
							case "Ugrave":         index = 217;  break;
							case "Uacute":         index = 218;  break;
							case "Ucircumflex":    index = 219;  break;
							case "Udieresis":      index = 220;  break;
							case "Yacute":         index = 221;  break;
							case "Thorn":          index = 222;  break;
							case "agrave":         index = 224;  break;
							case "aacute":         index = 225;  break;
							case "acircumflex":    index = 226;  break;
							case "atilde":         index = 227;  break;
							case "adieresis":      index = 228;  break;
							case "aring":          index = 229;  break;
							case "ccedilla":       index = 231;  break;
							case "egrave":         index = 232;  break;
							case "eacute":         index = 233;  break;
							case "ecircumflex":    index = 234;  break;
							case "edieresis":      index = 235;  break;
							case "igrave":         index = 236;  break;
							case "iacute":         index = 237;  break;
							case "icircumflex":    index = 238;  break;
							case "idieresis":      index = 239;  break;
							case "eth":            index = 240;  break;
							case "ntilde":         index = 241;  break;
							case "ograve":         index = 242;  break;
							case "oacute":         index = 243;  break;
							case "ocircumflex":    index = 244;  break;
							case "otilde":         index = 245;  break;
							case "odieresis":      index = 246;  break;
							case "divide":         index = 247;  break;
							case "ugrave":         index = 249;  break;
							case "uacute":         index = 250;  break;
							case "ucircumflex":    index = 251;  break;
							case "udieresis":      index = 252;  break;
							case "yacute":         index = 253;  break;
							case "thorn":          index = 254;  break;
							case "ydieresis":      index = 255;  break;
							case "Amacron":        index = 256;  break;

							case "Abreve":         index = 258;  break;
							case "abreve":         index = 259;  break;
							case "Aogonek":        index = 260;  break;
							case "aogonek":        index = 261;  break;
							case "Cacute":         index = 262;  break;
							case "cacute":         index = 263;  break;

							case "Ccaron":         index = 268;  break;
							case "ccaron":         index = 269;  break;
							case "Dcaron":         index = 270;  break;
							case "dcaron":         index = 271;  break;
							case "Dcroat":         index = 272;  break;
							case "dcroat":         index = 273;  break;
							case "Emacron":        index = 274;  break;
							case "emacron":        index = 275;  break;

							case "Edotaccent":     index = 278;  break;
							case "edotaccent":     index = 279;  break;
							case "Eogonek":        index = 280;  break;
							case "eogonek":        index = 281;  break;
							case "Ecaron":         index = 282;  break;
							case "ecaron":         index = 283;  break;

							case "Gbreve":         index = 286;  break;
							case "gbreve":         index = 287;  break;

							case "Gcommaaccent":   index = 290;  break;
							case "gcommaaccent":   index = 291;  break;
                    
							case "Imacron":        index = 298;  break;
							case "imacron":        index = 299;  break;
							case "Euro":           index = 128;  break;
							default:				index = 0;   break;
						}
						break;
					}
				}
			}
			
			if (index > 0) 
				afm.CharMetrics[index] = this;
		}
	}
	#endregion

}
