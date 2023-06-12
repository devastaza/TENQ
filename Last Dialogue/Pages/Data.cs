using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace CSharp_Shell
{

	public class Data
	{
		static Dictionary<string, object> objects = MainPage.profile.objects;
		public static readonly Dictionary<string, PageForm> dictionary = new Dictionary<string, PageForm>()
		{
            {
                "mainPage",     // Это имя страницы
                new PageForm
                {
                    subtitle = "Главное меню",
                    backgroundImageSource = new List<string>{"city.jpg"},
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Начать игру",         // Это текст кнопки. Можешь его отредачить, если считаешь нужным.
                            BindingContext = "firstPage"  // Это ссылка на другую страницу. По ней можешь понять, что откроется после нажатия.
                        },
                  	    new Button
                		{
                			Text = "Продолжить",
                			BindingContext = "Saves"
                		},
                		new Button
                        {
                            Text = "Предисловие",
                            BindingContext = "prologue"
                        },
                        new Button
                        {
                            Text = "Управление",
                            BindingContext = "tutorial"
                        },
                        new Button
                        {
                            Text = "Информация",
                            BindingContext = "about"
                        },
                        new Button
                        {
                            Text = "Выйти из игры",
                            BindingContext = "ExitTheGame"
                        }
                  },
                    music = "Musics/Empty Hope.mp3",
                    backgroundGifSource = "Snow.png",
                    fx = "stop",
                    settings = new PageFormSettings
                    {
                        subtitleLineAnimation = false,
                        IsSubtitleVisible = false,
                        isBgImageParallaxing = true,
                        hasSubtitleSaveInHistory = false,
                        buttonsXYoptions = new LayoutOptions[] {LayoutOptions.Center, LayoutOptions.CenterAndExpand}
                    },
                    extendedOptions = async page =>
                    {
                    	page.image.Source = ImageSource.FromResource ("resources.icons.text.png");
                    	page.imgField.IsVisible = true;
                    	page.addonLayout.Children.Clear();
                    	MainPage.profile.objects["scrollBack"] = false;
                        page.backgroundGif.ScaleY = 1.05;
                        while (MainPage.profile.objects["presentPage"].ToString() == "mainPage")
                        {
                            page.backgroundGif.TranslationY = -100;
                            page.backgroundGif.TranslationX = -20;
                            await page.backgroundGif.TranslateTo (20, 100);
                        }
                        page.backgroundGif.ScaleY = 1;
                        page.backgroundGif.TranslationX = page.backgroundGif.TranslationY = 0;
                    }
            	}
            },
            {
            	"Saves",
            	new PageForm
            	{
            		subtitle = "Сохранения",
            		imgSource = "none",
            		settings = new PageFormSettings
            		{
            			subtitleLineAnimation = false,
            			isBgImageParallaxing = true
            		},
            		extendedOptions = async page =>
            		{
            			var profile = new Profile();
            			profile = MainPage.profile;
            			
            			string[] allSavesName = Directory.GetFiles (SomeMethods.savesDirectory);
            			string currentFileName;
            			var savesLayout = new StackLayout();
            			
            			page.addonLayout.Children.Add (savesLayout);

            			for (int i = 0; i < allSavesName.Length; i++)
            			{
            				currentFileName = Path.GetFileName (allSavesName [i]).Replace(".json", null);
            				await SomeMethods.LoadGame (currentFileName);
            				
            				var frame = new Frame
            				{
            					Opacity = 0,
            					TranslationY = 80,
            					CornerRadius = 10,
            					Margin = 15,
            					BackgroundColor = Color.FromHex ("#00000000"),
            					HeightRequest = 80,
            					Padding = 0
            				};
            				
            				var img = new Image
            				{
            					WidthRequest = 130,
            					HeightRequest = 80,
            					Aspect = Aspect.AspectFill
            				};
            				if (MainPage.profile.lastImgSource != null && MainPage.profile.lastImgSource != "none")
            				{
            					img.Source = ImageSource.FromResource ("resources.Images." + MainPage.profile.lastImgSource);
            				}
            				else img.Source = ImageSource.FromResource ("resources.icons.text.png");

            				var fileName = new Label
            				{
            					Text = currentFileName,
            					TextColor = Color.White
            				};
            				
            				var subtitle = new Label
            				{
            					Text = MainPage.profile.lastSubtitle
            				};
            				
            				var creationDate = new Label
            				{
            					Text = File.GetCreationTime(allSavesName[i]).ToShortTimeString() + "\n" +
            						   File.GetCreationTime(allSavesName[i]).ToLongDateString(),
            					FontSize = 10,
            					HorizontalOptions = LayoutOptions.EndAndExpand,
            					HorizontalTextAlignment = TextAlignment.End,
            					VerticalOptions = LayoutOptions.End,
            					FontAttributes = FontAttributes.Italic
            				};
            				
            				var line = new BoxView
            				{
            					BackgroundColor = Color.White,
            					HeightRequest = 1,
            					HorizontalOptions = LayoutOptions.Fill
            				};
            				
            				var textsLayout = new StackLayout
            				{
            					Margin = new Thickness (10, 0, 0, 0),
            					Children = {fileName, line, subtitle},
            					VerticalOptions = LayoutOptions.Center
            				};
            				
            				var container = new StackLayout
            				{
            					Orientation = StackOrientation.Horizontal,
            					HorizontalOptions = LayoutOptions.FillAndExpand,
            					BackgroundColor = Color.FromHex("#363652"),
            					Children = {img, textsLayout, creationDate}
            				};
            				
            				var recognizer = new TapGestureRecognizer();
            				
            				recognizer.Tapped += async (s, e) =>
            				{
            					await SomeMethods.LoadGame (fileName.Text);
            					page.addonLayout.Children.Clear();
            					page.RefreshPage (MainPage.profile.objects["presentPage"].ToString());
            					frame.GestureRecognizers.Clear();
            				};
            				
            				frame.GestureRecognizers.Add (recognizer);
            				
            				frame.Content = container;
            				savesLayout.Children.Add (frame);
            				
            				frame.FadeTo (1, 500, Easing.CubicInOut);
            				frame.TranslateTo (0, 0, 500, Easing.CubicInOut);
            				await Task.Delay (50);
            			}
            			
            			MainPage.profile = profile;
            		}
            	}
            },
            {
            	"about",
            	new PageForm
            	{
            		subtitle = "Техническая информация",
            		backgroundGifSource = "Snow.png",
            		link = "mainPage",
            		texts = new List<string>
            		{
            			"Игра написана на собственном движке \"tenq\" на языке C# при помощи библиотеки Xamarin Forms.\n\n"
            			+ "Код пишется в одиночку на смартфоне. Цель - развитие способностей.\nЕсли Вы нашли баги/недочёты, прошу оповестить меня о них.\n\n"
            			+ "Тестер - Эмилька сан\nРедактор - ayanami\nИдея - devastaza\n\n"
            			+ "Плагины:\n	•Xamarin.Forms 4.4.0 pre-2\n	•CrossCurrentActivity //James Montemagno",
            			"Планы\n\n•Добавление озвучки персонажей\n•Добавление достижений\n•Перевод на английский язык\n•Добавление 3+ концовок\n•Выход в Play Market",
            			"Спасибо большое за то что Вы играете в этот текстовый квест. Если Вам не сложно, оставьте пожалуйста ваши впечатления/пожелания на форуме.\n\nХорошего времяпровождения!"
            		},
            		settings = new PageFormSettings
            		{
            			textFieldColor = "#77000000",
            			subtitleLineAnimation = false
            		},
                    extendedOptions = async page =>
                    {
                        page.backgroundGif.ScaleY = 1.05;
                        while (Data.objects["presentPage"].ToString() == "about")
                        {
                            page.backgroundGif.TranslationY = -100;
                            page.backgroundGif.TranslationX = -20;
                            await page.backgroundGif.TranslateTo (20, 100);
                        }
                        page.backgroundGif.ScaleY = 1;
                    }        
                }
            },
            {
                "tutorial",   //  Это тоже имя страницы
                new PageForm
                {
                    subtitle = "Управление",
                    imgSource = "none",
                    backgroundGifSource = "Snow.png",
                    texts = new List<string>                  // В "texts" сидит основной текст. Редачить его.
                    {
                        "Нажмите на любую незанятую часть экрана, чтобы продолжить чтение. Также при повторном касании пропускается анимация.",
                        "Нажмите дважды по этому тексту, чтобы открыть историю.",
                        "В ней вы можете перемотать историю к тому моменту, на какой текст вы нажмёте.\nУчтите, что в некоторых моментах эта функция недоступна.", //  \n означает что текст перешел на новую строку
                        "Историю можно закрыть, нажав на кнопку \"Назад\" на вашем смартфоне.",
                        "Для открытия меню нажмите на заголовок. Он находится над этим текстом.",
                        "Меню можно вызвать и закрыть при помощи кнопки \"Назад\" на вашем смартфоне.",
                        "Вы можете дважды нажать на фотографию и детально её просмотреть. Для сворачивания также дважды нажмите на фотографию.",
                        "Также она сама свернется, если вы продолжите чтение.",
                        "Игра поддерживает горизонтальный режим. Только не забудьте включить автоповорот в настройках вашего смартфона.",
                        "На этом всё, удачи!"
                    },
                    link = "mainPage",
                    settings = new PageFormSettings
                    {
                        subtitleLineAnimation = false
                    },
                    extendedOptions = async page =>
                    {
                        page.subtitle.Text = "Управление";
                        objects["лояльность"] = 10;
                        page.backgroundGif.ScaleY = 1.05;
                        while (Data.objects["presentPage"].ToString() == "tutorial")
                        {
                            page.backgroundGif.TranslationY = -100;
                            page.backgroundGif.TranslationX = -20;
                            await page.backgroundGif.TranslateTo (20, 100);
                        }
                        page.backgroundGif.ScaleY = 1;
                    }
                }
            },
            {
                "prologue",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Игра рассказывает про простого фрилансера, в жизни которого не происxодит ничего интересного. Однажды он, вернувшись с собеседования, увидел такую картину: дом, в котором он живет, облеплен группой людей, повсюду можно видеть полицейскиx и все они смотрят на крышу.",
                        "На ней же сидит незнакомый человек. Кажется, он не в лучшем состоянии. Печально, ведь наш главный герой не сможет попасть к себе в квартиру. Придется ждать..."
                    },
                    imgSource = "none",
                    link = "mainPage",
                    backgroundGifSource = "Snow.png",
                    settings = new PageFormSettings
                    {
                        subtitleLineAnimation = false
                    },
                    extendedOptions = async page =>
                    {
                        page.backgroundGif.ScaleY = 1.05;
                        while (Data.objects["presentPage"].ToString() == "prologue")
                        {
                            page.backgroundGif.TranslationY = -100;
                            page.backgroundGif.TranslationX = -20;
                            await page.backgroundGif.TranslateTo (20, 100);
                        }
                        page.backgroundGif.ScaleY = 1;
                    }
                }
            },
            {
                "firstPage",
                new PageForm
                {
                    backgroundImageSource = new List<string>{"bgStreet.png"},
                    subtitle = "Дорога домой",
                    texts = new List<string>
                    {
                        "Сколько шума... Оживленная толпа перебиралась с места на место, горячо обсуждая какую-то тему. Стражи порядка гоняли их, дабы те не мешались под ногами. А я что? Я просто рассматривал всю ситуацию издалека.",
                        "Не люблю группы людей, а тут их в большом количестве. И вся эта масса следит за кем-то, кто сидит на крыше высотного здания, свесив ноги. Кажется, это девушка, не могу разглядеть."
                	},
                    imgSource = "cops.jpg",
                    link = "secondPage",    // Это тоже ссылка на новую страницу. На странице бывают либо кнопки со ссылками, либо же прямые ссылки такого типа, как здесь
                    music = "Musics/Forgotten Life.mp3",
                    fx = "FX/crowd.opus",
                    settings = new PageFormSettings
                    {
                        isFXlooping = true
                    },
                    extendedOptions = page =>
                    {
                        objects["scrollBack"] = true;
                        page.DisplayAlert (null, "Советуем использовать наушники для большего погружения в историю.", "ок");
                    }
                }
            },
            {
                "secondPage",
                new PageForm
                {
                    backgroundImageSource = new List<string>{"bgStreet.png"},
                    texts = new List<string>
                    {
                        "И почему никто не хочет ей помочь? За все время я не увидел ни пожарников, ни парамедиков. Одни полицейские со злостным тоном просят граждан отойти подальше от здания.\n\nУ каждого второго камера смартфона направлена на крышу здания; люди смеются, волнуются, плачут. У каждого своя реакция на разные поступки.\nУхх... Серая масса.",
                        "Как назло, моя квартира находится именно в этом доме, и, разумеется, меня туда не впустят. Придется смотреть на этот грязный цирк до конца.",
                        "Я присел у тротуара, дожидаясь, пока что-то случится. Я бы конечно с радостью помог тому человеку, но... Кто я такой? Простой фрилансер, который сам пару раз встречался с теми же мыслями, что у того человека на крыше. Как и чем я смогу ей помочь?",
                        "Недалеко от меня шагал инспектор и что-то докладывал в свою рацию...",
                        "- [Рация] Ты сообщил Демидову?\n- [Инспектор] Да, Кирилл сказал, что пробки, будет нескоро.\n- [Рация] Чёрт... Ладно, сообщи нашим о нём! А то ещё своего же не пустят.\n- [Инспектор] Есть!\n\nНет, и это разве нормально?! Тут секунда на вес золота, а они тут прохлаждаются.",
                        "Любопытство взяло надо мной верх и я решил таки спросить у инспектора чего они ждут."
                    },
                    imgSource = "crowd.jpg",
                    link = "thirdPage"
                }
            },
            {
                "thirdPage",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Эй, кхм... Товарищ, погодите!\n- [Инспектор] Да, что такое?\nМогу я узнать чего вы дожидаетесь? Тот человек может спрыгнуть в любую секунду!\n- [Инспектор] Думаешь, мы сами не знаем?\n- Ну так сделайте что-нибудь! Уже сколько я тут стою, ни одна бригада не приехала. Отправьте хоть кого-нибудь на крышу, пусть попробует предотвратить ЧП.",
                        "- [Инспектор] Да мы с радостью, но пожарные части заняты. Недалеко воспламенился какой-то обширный завод, так и вся местная часть подала силы туда. Звоним в скорую, сказали что выслали психолога и парамедиков, но, учитывая трафик, приедут они нескоро. Так, погоди, меня тут вызывают..."
                    },
                    imgSource = "cop.jpg",
                    link = "fourthPage"
                }
            },
            {
				"fourthPage",
				new PageForm
				{
					texts = new List<string>
					{
						"Мужчина отошел со своей говорилкой в сторону полицейской машины, оставив меня снова одного со своими мыслями. Трафик и правда плотный. Вечернее время, все едут с работы по домам, а тут еще и недалеко от центра. Пробка на пробке.\nБлин, психолога они ждут...",
						"Мне так жалко того человека. Я представляю себя на его месте. Просто... Кроме смерти и всей своей жизни он ни о чем не думает. Знала бы толпа, как ему сейчас тяжело. Могли бы они прочитать его мысли, они бы расплакались.",
						"Эти люди думают что он не хочет жить. Бред! Именно сейчас он ищет в себе причины остаться в этом мире. Именно сейчас он ищет в своей прожитой жизни что-то приятное и счастливое. Перед его глазами весы. В одной чаше вся боль, что ему причинили, а в другой все самое счастливое. И если вторая чаша поднимется...",
						"С другой стороны... Почему я так завёлся? Этот человек мне ни родственник, ни знакомый, ни близкий, как и я ему. Да, я его прекрасно понимаю, но мне же никто не помогал.\nСпокойно, Артём, спокойно. Скоро всё это закончится.",
						"Ну это возмутительно. Двадцать минут прошло, а этого психолога тут только и слышали. За это время меня постигла одна хитрая мысль. Я знаю, эти мысли меня никогда не доводили до успеха, но... Я мог бы представиться тем самым психологом и... Помочь? Ну серьезно, еще немного, и та бедняжка спрыгнет с крыши! Меня терзала эта мысль, и вскоре я сам не заметил, как пошел в сторону двери здания. Там меня ожидали пару офицеров, которые сразу принялись меня останавливать.",
					},
					imgSource = "crowd.jpg",
					link = "fifthPage"
				}
			},
            {
                "fifthPage",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Так, всё, парни, я пришёл!\n- [Коп] Гражданин, вы кем являетесь?\n- Как кем? Психолог я! Давайте, не задерживайте меня.\n- [Коп] Предъявите ваши документы, пожалуйста.\n\nВот этого я и боялся... Судорожно пройдясь руками по карманам, имитируя растёпу, я сказал:"
                    },
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Кажется, они в машине",
                            BindingContext = "oniVmawine"
                        },
                        new Button
                        {
                            Text = "Видимо на работе оставил",
                            BindingContext = "naRabote"
                        }
                    },
                    imgSource = "two_cops.jpg"
                }
            },
            {
                "naRabote",
                new PageForm
                {
                    imgSource = "cop.jpg",
                    texts = new List<string>
                    {
                        "- Видимо в спешке оставил в своём кабинете. Да Бог с этими документами, пропустите!\n-[Офицер] А где же твоя бригада?\n- А, ну, так это, пробки! Я пешком пошёл, они скоро доберутся.\n- [Офицер] Ах вот как. В любом случае без документов ни шагу! В кабинете забыли? Может, по дороге потеряли?\n- Вы издеваетесь что ли? Я помню, что в последний раз оставлял их на работе, за столом кажется.\n- [Офицер] На столе, значит? Хорошо...\n\nЭто мне не нравится...",
                        "Офицер оставил меня со своим собратом, достал телефон и кому-то позвонил.",
                        "- [Офицер] Аллё, Степаныч! Как жизнь?\n- [Трубка] ...\n\nВот чёрт, разговор вообще не слышен.\n\n- [Офицер] Да вот тоже так. Слушай, заскочи к вашему психологу.\n- [Трубка] ...\n- [Офицер] Да-да, тот что новенький, вчера к вам переквалифицировался. Посмотри, нет ли на столе его паспорта и прочих бумаг, да и сам когда прибудет?\n\nКажется, я попал... И вот надо было мне про стол сморозить?\n\n- [Трубка] ...\n- [Офицер] Всё, понял. Ну давай там, Ульке привет от меня!",
                        "Офицер направился обратно к нам, ехидно оскалившись на меня.\n- [Офицер] Нет на столе ваших документов, к тому же сам психолог с бригадой до сих пор в пробке.\n- А... Вот как.\n\nСтало до жути неудобно. Вот бы сейчас провалиться под землю."
                    },
                    link = "failPage",
                    extendedOptions = page =>
                    {
                        objects["scrollBack"] = false;
                    }
                }
            },
            {
                "failPage",
                new PageForm
                {
                    subtitle = "Провал",
                    texts = new List<string>
                    {
                        "- [Офицер] Эй, Семёнов, отведи-ка этого гражданина с глаз моих долой, и протокол заполни.\n- [Инспектор] Хорошо, а что он сделал?\n- [Офицер] Да за дураков нас держит, психологом прикинулся.\n- [Инспектор] А что н... А, понял. Ну, гражданин, прошу последовать за мной!\n\nМне больше ничего не оставалось и я, опустив голову от стыда, последовал за инспектором.",
                        "Пришлось вместе со всеми ждать конца этого дурдома. Стоит ли мне говорить, чем всё закончилось?"
                    },
                    link = "mainPage",
                    imgSource = "none",
                    extendedOptions = page =>
                    {
                        objects["scrollBack"] = false;
                    }
                }
            },
            {
				"oniVmawine",
				new PageForm
				{
					texts = new List<string>
					{
						"- Да и припарковались мы за два квартала отсюда.\n- [Офицер] А бригада твоя где?\n- Да там же, сейчас они подойдут. Полотно достают, медикаменты. Долго объяснять.\n- [Офицер] Без документов ни шагу ко двери!",
						"- Пока я туда-сюда буду круги нарезать, может произойти ЧП!\n\nКажется офицера это ничуть не колышет."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Объяснить",
							BindingContext = "obyasnit"
						},
						new Button
						{
							Text = "Надавить",
							BindingContext = "nadavit"
						}
					}
				}
			},
			{
				"nadavit",
				new PageForm
				{
					imgSource = "cop.jpg",
					texts = new List<string>
					{
						"- Слышь ты, в фуражке! Я тут вообще-то пришел жизнь человеку спасти, а не выслушивать твою заученную речь. Умрет она и виноват будешь здесь только ты! Быстро пропустил меня пока я на тебя заяву не накатал!",
						"Офицер все также стоял передо мной горой и повторял одно и то же.\n\n- [Офицер] Без нужных документов не пропущу. И следует вам относиться к органам правопорядка более уважительно, иначе...\n- Иначе что?",
						"- [Офицер] Хочешь лично узнать?!\n- Боже мой, есть ли в этом мире хоть что-то, с чего вы не имеете выгоду?\n- [Офицер] Так, ещё одно слово и полетишь отсюда в КПЗ. Я сказал.",
						"Мда, таким настроем я ничего не добьюсь. Может стоит попробовать другой метод запудривания?"
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Объяснить",
							BindingContext = "obyasnit"
						}
					}
				}
			},
			{
				"obyasnit",
				new PageForm
				{
					imgSource = "cop.jpg",
					texts = new List<string>
					{
						"- Слушай, офицер, ты человек или зверь? У тебя есть чувства, нет? Представь что там, над нами сидит родной тебе человек. Он ждет помощи, поддержки, а ты эту самую помощь задерживаешь. Сейчас каждая минута дорога, как никогда! Пропусти, после я тебе хоть копию своих документов сделаю!\n- [Офицер] ...",
						"- [Офицер] Далеко говоришь припарковались?\n- Да говорю же, на Зыхском проспекте.\n- [Офицер] Хорошо, хоть имя с фамилией своё назови.\n\nТак, как там его по рации называли..."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Алексей Давидов",
							BindingContext = "failPage"
						},
						new Button
						{
							Text = "Кирилл Демидов",
							BindingContext = "trueName"
						},
						new Button
						{
							Text = "Константин Лермидов",
							BindingContext = "failPage"
						}
					}
				}
			},
			{
				"trueName",
				new PageForm
				{
					imgSource = "none",
					texts = new List<string>
					{
						"Офицер заглянул в свой блокнот, начал что-то вычитывать оттуда. Наверное им до этого сообщили как зовут психолога.",
						"- [Офицер] Кирилл значит... Хорошо, вроде так тебя и зовут. Значит так, Кирилл! Слушай меня внимательно.",
						"- [Офицер] При любой удобной возможности схватывай её, не нужно церемониться, у нас и так дел по горло. Как схватишь - дай знак в рацию. Канал то хоть помнишь?\n- Да-да, постараюсь не медлить.\n\nО какой рации он говорит?",
						"- [Офицер] И без фокусов тут! Вся ответственность сейчас на твоих плечах. Ты же у нас новенький? Вот и постарайся не провалиться в первой же смене.\n\nОфицер оглянулся по сторонам.",
						"- [Офицер] Чёрт, где же твою бригаду носит? Ай ладно, всё, давай! Олег! Сопроводи его.\n- [Второй офицер] Так точно! Хорошо, давайте за мной.",
						"Неужели повезло?"
					},
					link = "enterTheTower1"
				}
			},
			{
				"enterTheTower1",
				new PageForm
				{
					texts = new List<string>
					{
						"Офицер великодушно открыл передо мной дверь и я прошел внутрь, а за мной хвостиком последовал и он."
					},
					fx = "FX/metalDoorOpening.mp3",
					backgroundImageSource = new List<string>{"bgPadik.png"},
					link = "enterTheTower2"
				}
			},
			{
				"enterTheTower2",
				new PageForm
				{
					texts = new List<string>
					{
						"Гул сирен и топота притупился, стало немного тише, спокойнее. Какое блаженство.",
						"Вот только его нарушает одна личность, сверлящая мою спину взглядом. Как бы отвязаться от него...",
						"- Слушай, Олег. Я знаю как добраться, не нужно провожать.\n- [Олег] Увы, мне был выдан приказ.\n- Да я тебя умоляю. Думаешь твоему командиру нужно чтобы ты меня проводил? Да и во-вторых, мне нужно сосредоточиться.\n- [Олег] ...",
						"- [Олег] Я тут уже часа три стою, а момента пойти отлить никак не появлялся. Давай так. Если вдруг кто спросит - поднялись мы вместе!\n- Да не вопрос! Всё, давай, а то затопишь тут нас, кхе."
					},
					fx = "FX/metalDoorClosing.mp3",
					link = "enterTheTower3"
				}
			},
			{
				"enterTheTower3",
				new PageForm
				{
					texts = new List<string>
					{
						"Олег умчался ко ступенькам на второй этаж. Интересно, куда это он... Впрочем, от копа отвязался.",
						"Все так до тошноты знакомо. Эти облезлые темно-синие стены, паутины в уголках, холодный, тусклый, словно мертвый свет от лампочки. Я как будто в загробной реанимации. Так! Не время для раздумий!",
						"Я вызвал лифт. И как всегда эта чёртова кнопка не срабатывает с первого раза. Хотя на что я срываюсь, она уже отработала своё.",
						"Ай, слишком сильно нажал, больно!",
						"Я услышал характерный грохот с верхних этажей. Вот он, родимый, ко мне спускается.",
						"Лифт открылся передо мной и я нырнул в него."
					},
					link = "enterTheLift1",
					imgSource = "lift.jpg",
					fx = "FX/copOnStairs.mp3"
				}
			},
			{
				"enterTheLift1",
				new PageForm
				{
					link = "enterTheLift2",
					imgSource = "liftButtonNotPressed.jpg",
					settings = new PageFormSettings
					{
						imageSize = new int[] {400,700}
					}
				}
			},
			{
				"enterTheLift2",
				new PageForm
				{
					link = "mejLiftomIkriwey",
					imgSource = "liftButtonPressed.jpg",
					settings = new PageFormSettings
					{
						imageSize = new int[] {400,700}
					},
					fx = "FX/inLift.mp3"
				}
			},
			{
				"mejLiftomIkriwey",
				new PageForm
				{
					texts = new List<string>
					{
						"Двери со скрипом разошлись, открывая мне путь к лестнице.",
						"Не в небеса конечно, но близко."
					},
					imgSource = "none",
					music = "stop",
					link = "stairs",
					fx = "FX/outLift.mp3"
				}
			},
			{
				"stairs",
				new PageForm
				{
					texts = new List<string>
					{
						"Ухх, холодные металичесские поручни. Отрезвляют разум, что мне сейчас и необходимо.",
						"Осталось лишь откинуть крышку люка.",
						"Фух... И-и-и..."
					},
					fx = "FX/climbTheStairs.mp3",
					link = "hatch"
				}
			},
			{
				"hatch",
				new PageForm
				{
					texts = new List<string>
					{
						"Раз!",
						"Ага, а я то ещё в форме, хе-хе.",
						"Так.",
						"Похоже...",
						"Похоже что я смог это сделать.",
						"Я смог добраться до этого места!"
					},
					fx = "FX/openTheHatch.mp3",
					link = "roof"
				}
			},
			{
				"roof",
				new PageForm
				{
					backgroundImageSource = new List<string>{"bgRoof.png"},
					subtitle = "Где всё и решится",
					imgSource = "roof.jpg",
					music = "Musics/Confusion.mp3",
					link = "roof2",
					texts = new List<string>
					{
						"Вот он, человек, сидящий на обрыве своей судьбы.",
						"Хорошо, он всё ещё здесь.",
						"Всё же я не ошибся, это девушка. В любом случае пора начать решающий диалог с незнакомкой.",
						"Я тихо двинулся к ней. Та уловила мои шаги и прокричала:"
					}
				}
			},
			{
				"roof2",
				new PageForm
				{
					imgSource = "none",
					texts = new List<string>
					{
						"- [Девушка] ЕЩЁ ОДИН ШАГ И Я СПРЫГНУ!!!",
						"- Слушай, я не собираюсь насильно тебя останавливать, я хочу с тобой поговорить.",
						"- [Девушка] А Я НЕ ХОЧУ!",
						"- Просто разреши мне присесть неподалеку. Я клянусь что буду без резких движений и принуждений.",
                        "Девушка ничего не ответила. Она лишь сидела неподвижно, пока по ее щекам катились слезы. Я робко двинулся к концу крыши и уселся сбоку от незнакомки.\n\n Нужно как-то начать разговор..."
                    },
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Я такой же, как и ты",
							BindingContext = "Я такой же, как и ты"
						},
						new Button
						{
							Text = "Суицид не выход",
							BindingContext = "живи до суицида"
						},
						new Button
						{
							Text = "Тебя семья внизу ждёт",
							BindingContext = "Доминик торренто"
						}
					}
				}
			},
			{
				"живи до суицида",
				new PageForm
				{
					imgSource = "roof.jpg",
					texts = new List<string>
					{
						"- [Девушка] Ты даже представить себе не можешь сколько раз мне это говорили.\n- Но это правда так!",
                        "- [Девушка] Я устала... Устала от своих проблем. По крайней мере смерть позволит мне оставить все позади.",
						"- Только из-за проблем ты хочешь наложить на себя руки? Не считаешь это каким-то неправильным?",
						"- [Девушка] Вся моя жизнь какая-то неправильная. Не читай мне мораль пожалуйста, мне её по горло досталось от лжецов и предателей.",
						"Не стоило мне про это говорить. Тем не менее, надо продолжать диалог."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Здесь спокойно",
							BindingContext = "спокойно"
						},
						new Button
						{
							Text = "Здесь холодно",
							BindingContext = "холодно"
						}
					}
				}
			},
			{
				"Доминик торренто",
				new PageForm
				{
					imgSource = "roof.jpg",
					texts = new List<string>
					{
						"- Тебя внизу семья дожидается, все волнуются.",
						"- [Девушка] Меня обманываешь или себя?",
						"- Нет, я правда!",
						"- [Девушка] И зачем тебя сюда отправили...",
						"Неловко-то как. Ей Богу, хоть сам сейчас возьми и спрыгни...",
						"В любом случае ложь мне сейчас не поможет. Нужно сменить тему."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Здесь спокойно",
							BindingContext = "спокойно"
						},
						new Button
						{
							Text = "Здесь холодно",
							BindingContext = "холодно"
						}
					}
				}
			},
			{
				"Я такой же, как и ты",
				new PageForm
				{
					imgSource = "roof.jpg",
					texts = new List<string>
					{
						"- Знаешь, я часто сюда поднимался с теми же мыслями, что и у тебя. Долго сидел и думал, не привлекая внимание публики.",
						"- [Девушка] ..."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Здесь спокойно",
							BindingContext = "спокойно"
						},
						new Button
						{
							Text = "Здесь холодно",
							BindingContext = "холодно"
						}
					}
				}
			},
			{
				"холодно",
				new PageForm
				{
					texts = new List<string>
					{
						"- Холодный сегодня денёк выдался, до костяшек продрог. Может, спустимся вниз?",
						"Ой...",
						"- [Девушка] Если ты пытаешься уговорить меня спуститься к людям, то даже не пытайся. Твои шансы равны нулю. К тому же, тебя никто не держит.",
						"- И в мыслях не было! Я просто... Прости. мне порой сложно выражать то, о чём я думаю.",
						"- [Девушка] ...",
						"Чёрт, она не так меня поняла! Впрочем, сам виноват. Но ладно, останавливаться нельзя."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Посмотри вниз",
							BindingContext = "посмотри"
						},
						new Button
						{
							Text = "Зачем тебе это?",
							BindingContext = "зачем?"
						}
					}		
				}
			},
			{
				"спокойно",
				new PageForm
				{
					texts = new List<string>
					{
						"- Звуки уличной жизни сюда еле доходят. Только ветер сегодня явно разбушевался что-то.",
						"- [Девушка] Да, здесь спокойно..."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Посмотри вниз",
							BindingContext = "посмотри"
						},
						new Button
						{
							Text = "Зачем тебе это?",
							BindingContext = "зачем?"
						}
					}
				}
			},
			{
				"зачем?",
				new PageForm
				{
					texts = new List<string>
					{
						"- Ты же кроме смерти ничего не добьёшься.",
						"- [Девушка] А мне кроме смерти ничего и не надо!",
						"- Как так? Ты же ведь только начала свою линию жизни!",
						"- [Девушка] Нажилась досыта! Прекрати меня переубеждать.",
						"- Как скажешь...",
						"Неудача. Нужно поговорить о чём-то другом, сменить тему."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Родные волнуются",
							BindingContext = "родныеДрузья"
						},
						new Button
						{
							Text = "Друзья волнуются",
							BindingContext = "родныеДрузья"
						}
					}		
				}
			},
			{
				"посмотри",
				new PageForm
				{
					texts = new List<string>
					{
						"- Посмотри сколько людей внизу собралось. Все они здесь из-за тебя.",
                        "- [Девушка] Им всем все равно на меня. Они тут лишь ради того, чтобы запечатлеть кадр на своём телефоне."
                    },
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Родные волнуются",
							BindingContext = "родныеДрузья"
						},
						new Button
						{
							Text = "Друзья волнуются",
							BindingContext = "родныеДрузья"
						}
					}
				}
			},
			{
				"родныеДрузья",
				new PageForm
				{
					texts = new List<string>
					{
						"- Тебя по телевизору будут показывать. Не думаешь, что близкие тебе люди начнут плакать?",
						"- [Девушка] Их нет.",
						"- Ох, прости меня пожалуйста...",
						"- [Девушка] Бывает.",
						"Неловкое молчание. Девушка была погребена в своих мыслях. Нужно разговорить её."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "А как тебя зовут?",
							BindingContext = "как звать?"
						}
					}
				}
			},
			{
				"как звать?",
				new PageForm
				{
					texts = new List<string>
					{
						"- [Девушка] Никак.",
						"- В смысле?",
						"- [Девушка] В прямом! Да и разве это имеет какое-то значение?",
						"- Ну... Всё в этом мире имеет своё значение, причину своего существования.\n- [Девушка] Очевидно я родилась без этого.",
						"- Нет, такого не бывает. У всего есть своё предназначение.\n- [Девушка] Видимо, моё предназначение - это создавать всем проблемы.",
						"- Да глупость! Я уверен, в твоей жизни были интересные и весёлые периоды.\n- [Девушка] Почему ты так в этом уверен?",
						"- Ну... Я слышал много людских историй и в каждой были хорошие моменты, нечто светлое, тёплое.",
                        "Девушка грустно хмыкнула и продолжила смотреть в никуда.",
						"Прошла ещё одна затяжная минута. Молчать нельзя!"
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Представиться",
							BindingContext = "представиться"
						}
					}
				}
			},
			{
				"представиться",
				new PageForm
				{
					texts = new List<string>
					{
						"- Меня зовут Артём.",
						"- [Девушка] Ага...",
                        "Кажется ее это ни капли не интересовало. Погода ухудшалась, словно она проживает ситуацию вместе с нами.",
						"Я вдруг заметил, что девушка дрожит. Не знаю, от холода ли, или от эмоций. Тем не менее на крыше действительно холодно."
					},
					buttons = new List<Button>
					{
						new Button
						{
							Text = "Отдать куртку",
							BindingContext = "kurtka"
						},
						new Button
						{
							Text = "Подсесть ближе",
							BindingContext = "dvijene"
						}
					}
				}
			},
			{
				"dvijene",
				new PageForm
				{
					imgSource = "none",
					texts = new List<string>
					{
						"Я поднялся с края крыши и направился в сторону девушки.",
						"- [Девушка] Что ты задумал?",
						"- Ничего. Я увидел, что тебе холодно и решил подсесть.",
						"- [Девушка] Ты ошибаешься, мне не холодно. Сядь обратно, пожалуйста...",
						"- Хорошо. Видишь, я не пришёл снять тебя отсюда любой ценой.",
						"- [Девушка] Да, я вижу."
					},
					link = "наконец то"
				}
			},
			{
				"kurtka",
				new PageForm
				{
					imgSource = "none",
					texts = new List<string>
					{
                        "Я решил снять с себя куртку и накинуть на плечи незнакомки. Она промолчала, лишь потянув воротники, чтобы защититься от ветра.",
						"- [Девушка] Зачем ты это делаешь?\n- Я хочу помочь.\n- [Девушка] Врёшь, тебе за это платят!\n- Милая, я работаю фрилансером. Да и попал сюда, лишь немного одурачив копов и благодаря моей сегодняшней неимоверной удаче.",
						"- [Девушка] Не верю!",
						"- Мне незачем врать тебе. К тому же внизу меня ждут проблемы, ибо я представился тем, кем не являюсь.",
						"Интересно, он вообще приехал?",
						"- [Девушка] И зачем же?\n- Я сам был в таком состоянии и прекрасно понимаю твою боль. Всмысле я не могу почувствовать это в той же степени что и ты, но примерно догадываюсь.",
						"И снова неловкое молчание. Девушка в своем мире.",
						"Только я подумал что ничем не смогу помочь, как тихим дрожащим голосом произнеслось",
						"- [Девушка] Спасибо...",
						"- Не за что. Могу и за шапкой с перчатками сходить.\n- [Девушка] Сходить?\n- Да, я живу шестью этажами ниже.\nНемного улыбнувшись, девушка сказала\n- [Девушка] Нет, не стоит."
					},
					link = "наконец то"
				}
			},
			{
				"наконец то",
				new PageForm
				{
					texts = new List<string>
					{
						"Тихо. Гул под нашими ногами немного утих. Все замерли в ожидании решения. Всю улицу освещают синие и красные огоньки мигалок. Сирены утихли, только ветер злился и бился об здание, попутно задевая нас.",
						"- [Девушка] Саша моё имя.",
						"- О, очень красивое имя. Очень приятно, Александра!",
                        "- [Саша] Ты издеваешься? Простое мальчишеское имя.",
						"- Ну и кто тебе это сказал?",
						"- [Саша] Да все!",
						"- Саш, это субъективное мнение. Оно имеет свойство противоречить нашим желаниям, а порой и нам самим.",
						"- Лично мне очень нравится твое имя. Оно... При произношении я представляю красивую, стройную девушку.",
						"- [Саша] Чего нельзя отнести ко мне.\n- Это не так.\n- [Саша] Врёшь!\n- Со стороны лучше видно.",
                        "Девушка отвернулась, я заметил как она прячет улыбку. Но вскоре все было также, как и до этого. Её окружали холод и мысли.",
						"Я решил снова начать разговор."
					},
					link = "13 причин"
				}
			},
			{
				"13 причин",
				new PageForm
				{
					texts = new List<string>
					{
						"- Саш, раз уж я тут... Может, расскажешь?",
						"- [Саша] О чём?\n- Ну, о том самом.",
						"Девушка вопросительно на меня посмотрела.\n- [Саша] Не поняла.",
						"- Почему ты решилась на это?\n- [Саша] Ты хочешь узнать причины моего желания умереть?\n- В точку!\n- [Саша] Ахх, это нудно и неинтересно.\n- Нет, я хочу услышать.",
						"- [Саша] ...",
						"- [Саша] Ладно... Но я и не удивлюсь, если ты встанешь и уйдёшь.\n- Не уйду.\n- [Саша] Уже слышала...",
						"Саша перебирала дух и готовилась к длинному монологу. Я же готовился запомнить каждое значимое слово и вскоре переубедить её"
					},
					link = "катарсис"
				}
			},
            {
                "катарсис",
                new PageForm
                {
                    subtitle = "Катарсис",
                    texts = new List<string>
                    {
                        "- [Саша] И так. Проблемы берут свое начало с моего детства. Я родилась ребенком от любовника своей матери. Разумеется, отец не любил меня как родную дочь, а мать бросила меня, когда мне было девять месяцев отроду. Соответственно, учиться воспитанию пришлось самой, от одноклассников. Отцовской любви я не видела. Каждую неделю он приводил домой новую женщину и заставлял меня называть ее мамой.\n- Это печально.",
                        "- [Саша] Ага, особенно когда половину твоего города приходилось называть матерью. Надо мной усмехались из-за моего папаши. Поэтому и в школе у меня репутация была не супер. Хочешь услышать стандартную мысль моих одноклассников?\n- Давай.",
                        "- [Саша] *писклявым голосом* А чему она научится? Какой папаша, такая и дочь!\n- Понятно...",
                        "- [Саша] Ко мне даже некоторые учителя нормально не обращались. Но знаешь, что?\n- Что?",
                        "- [Саша] Я не впала в депрессию. Я просто жила с этим, смирилась. Приняла это как есть и продолжала существовать. Но это все еще только вершина айсберга...",
                        "За весь разговор девушка сильно разволновалась и только сейчас решила перевести дух."
                    },
                    link = "катарсис 2",
                    music = "Musics/Катарсис.mp3",
                }
            },
            {
                "катарсис 2",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Так вот. Мой папаша не имел стабильной работы, он подрабатывал чтобы на еду хватало и коммуналку. Но чаще он просто ночевал у кого-то и забивал на дочь.",
                        "- [Саша] Когда его выгоняли с очередной работы, он, разъярённый, приходил домой и крушил все вокруг.",
                        "- [Саша] Знаешь...",
                        "- [Саша] Это не обошло стороной и меня.",
                        "- [Саша] Поначалу, когда я была маленькая, я очень боялась этого человека, дрожала после каждых побоев, тихо хлипала под кроватью, чтобы он не слышал.",
                        "- [Саша] Потом же, немного повзрослев, я устала бояться. Я смирилась и с этим, больше не кричала когда он меня бил, не плакала где-то в углу от страха.",
                        "- [Саша] Он... он просто лишил меня страха ко всему. Я стала очень смиренной и бесстрашной.",
                        "- [Саша] Это единственное, что он мне подарил.",
                        "- [Саша] Он украл мой страх.",
                        "Ее пустой взгляд был устремлен на тучи, собравшиеся в небе; слезы бесшумно скатывались по ее лицу, пока она рассказывала мне свою историю."
                    },
                    link = "катарсис 3"
                }
            },
            {
                "катарсис 3",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Знаешь, и тут я не сдалась. Я продолжила жить в своем бытие. Я дышала, хоть и ненавидела свою натуру и свою обитель. Я поняла фразу \"дышать не значит жить\", и пропиталась ею. Просто жажды жить в моих глазах стало меньше.\n- Мне тебя так жаль...",
                        "- [Саша] Что мне с того? Сколько бы жалости ко мне не было, её невозможно променять на другую жизнь.\n- Это да... Прости, что перебил, продолжай.",
                        "- [Саша] Я удивлена, что ты еще не ушёл.\n- Я же сказал.\n- [Саша] Ну-ну. Так вот. Не было у меня ни семьи, ни друзей, ни смыслов и целей. Я просто существовала, пока не пришел он.\n- Парень?",
                        "- [Саша] В точку. Моя первая и последняя любовь. Он был прекрасным человеком, с хорошим характером и красивой внешностью. Он смог как-то вселить в меня веру в светлое будущее, что я не такая уж бесполезная личность.",
                        "- Ну это-же прекрасно!",
                        "- [Саша] Да, было прекрасно...",
                        "- [Саша] Он ушел также неожиданно, как и пришел. Я думала что он не такой как все... Я ещё так никогда не ошибалась.",
                        "- [Саша] Он... ",
                        "- [Саша] Он просто использовал меня для своих чувств. Ты это понимаешь? Быть для кого-то какой-то приятной игрушкой, когда ты ему даришь весь свой мир.",
                        "- Это бесчеловечно...",
                        "- [Саша] Да. Но я скучаю. Нет, не по тому парню, что живет сейчас, а по тому, кого он строил передо мной. Тот образ...\n- Я тебя понимаю...",
                        "- [Саша] Да... И знаешь, что самое обидное в моей жизни?",
                        "- Это еще не все?!",
                        "Девушка засмеялась. Кажется, смех это истерический.",
                        "- [Саша] Нет, мой дорогой визави, это ещё не все. Представь себе, что я не имела свободы. Я не гуляла ни с кем, не веселилась с классом до одиннадцатого класса. Папаша мне запрещал выходить из дома, запирал двери, запрещал говорить с парнями, многое запрещал...",
                        "- [Саша] И бесило меня то, что потом он сравнивал меня с обычной среднестатистической школьницей и бросал недостатки мне в лицо.\n- Это же несправедливо!",
                        "- [Саша] А теперь представь каково было мне!",
                        "Девушка явно волновалась. С каждым словом тон ее голоса повышался. Я же старался сохранять решительность.",
                        "- Неописуемо.",
                        "- [Саша] И ты мне говоришь, что я должна жить?! Ради какой причины? Что было светлого в моей жизни? Цель моего существования? Может я уже её выполнила и могу спокойно уйти!",
                        "- Саша, это не так.",
                        "- [Саша] Докажи мне это! Я верила многим, кто водил меня за нос, а теперь я хочу настоящих аргументов!"
                    },
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Ты должна жить потому что родилась",
                            BindingContext = "родилась"
                        },
                        new Button
                        {
                            Text = "Проблемы и есть часть жизни",
                            BindingContext = "проблемы жизнь"
                        },
                        new Button
                        {
                            Text = "Многие сейчас живут хуже тебя",
                            BindingContext = "хуже тебя"
                        }
                    }
                }
            },
            {
                "хуже тебя",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Это не аргумент! Да и к тому же, как ты можешь судить? У каждого человека своя боль. То что сейчас чувствую я, не чувствует никто!"
                    },
                    link = "пора действовать"
                }
            },
            {
                "проблемы жизнь",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Для чего мне такая жизнь?!",
                        "- Судьбу не выбирают.",
                        "- [Саша] Зато я могу выбирать ход судьбы!",
                        "- Именно, твоё право.",
                        "- [Саша] И ты не имеешь права отговаривать меня!"
                    },
                    link = "пора действовать"
                }
            },
            {
                "родилась",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Глупый аргумент если честно.",
                        "- Почему же?",
                        "- [Саша] Так я же ошибка природы. Не должна была рождаться!",
                        "- Чушь. Если бы не должна была, то и не родилась бы!",
                        "- [Саша] Лучше бы и не родилась! Зачем я появилась на этот свет вообще?!"
                    },
                    link = "пора действовать"
                }
            },
            {
                "пора действовать",
                new PageForm
                {
                	music = "stop",
                    texts = new List<string>
                    {
                        "- Послушай меня, пожалу",
                        "- [Саша] НЕ ХОЧУ!",
                        "Девушка зарылась головой в колени, закрывшись руками."
                    },
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Подойти и обнять",
                            BindingContext = "обнять"
                        },
                        new Button
                        {
                            Text = "Успокаивать на словах",
                            BindingContext = "на словах"
                        }
                    }
                }
            },
            {
                "на словах",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Саш, ты просто в отчаянии. Ты должна собраться с силами. Это просто житейские проблемы!",
                        "- [Саша] Артем, спасибо тебе конечно, но это всего лишь слова, не более. Давай посидим в тишине, я не хочу с кем-то говорить.",
                        "- Хорошо, я тебя понял."
                    },
                    link = "конец катарсиса"
                }
            },
            {
                "обнять",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Я медленно, почти бесшумно встал со своего места и подсел к девушке, медленно обняв ее.",
                        "- [Саша] Отпусти меня!",
                        "- Нет.",
                        "- [Саша] ОТПУСТИ ГОВОРЮ!",
                    },
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Не отпускать",
                            BindingContext = "не отпускать"
                        },
                        new Button
                        {
                            Text = "Отпустить",
                            BindingContext = "на словах"
                        }
                    }
                }
            },
            {
                "не отпускать",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Кричи сколько хочешь, я тебя не отпущу.",
                        "Девушка сжалась и расплакалась еще сильнее, я же обнял ее чуть крепче."
                    },
                    link = "конец катарсиса",
                    extendedOptions = page =>
                    {
                    	MainPage.profile.objects["scrollBack"] = true;
                    }
                }
            },
            {
                "конец катарсиса",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Прошло пару минут.",
                        "Саша успокоилась и вытирала рукавом свой красный нос.",
                        "Мы молча сидели и смотрели на небо, пока улица переливалась в красных и синих огоньках.",
                        "Я повернулся, посмотрел в её глаза и увидел усталость, боль и страх."
                    },
                    link = "тучи рассеялись"
                }
            },
            {
                "тучи рассеялись",
                new PageForm
                {
                    subtitle = "Тучи рассеялись",
                    music = "Musics/the clouds have cleared.mp3",
                    texts = new List<string>
                    {
                        "- [Саша] Что будет, если я спрыгну?",
                        "- Конец вселенной.",
                        "- [Саша] Что это значит?",
                        "- То, что умрет твоя вселенная.",
                        "- [Саша] Чёртов философ...",
                        "- А ещё умрёт частичка моей вселенной.",
                        "- [Саша] Почему же?",
                        "- Ну, ты оставила след в моей жизни и, если я потеряю тебя, в ней образуется дырка. Черная дыра во вселенной.",
                        "- [Саша] Ты правда чёртов философ! К тому же, что я значу для тебя? Мы знакомы от силы полчаса.",
                        "- Несмотря на это, ты для меня стала близкой личностью. Я очень хорошо понимаю тебя, твоё состояние.",
                        "- [Саша] Знаешь, многие мне говорили такое, в итоге через пару дней мы теряли контакт.",
                        "- Со мной сложно спорить.",
                        "- [Саша] А со мной бесполезно.",
                        "- Думаю, мы поладим.",
                        "...сказал я с улыбкой. Девушка тоже улыбнулась и опустила голову, смотря на свои руки."
                    },
                    backgroundImageSource = new List<string>{"NovelTime/noFace.jpg", "", "", "NovelTime/noEmotionsWithEyes.jpg", "", "NovelTime/noEmotions.jpg", "", "", "", "NovelTime/noFace.jpg", "", "NovelTime/sorrow.jpg", "", "NovelTime/noEmotionsWithEyes.jpg", "", "NovelTime/smile.jpg"},
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Что с твоими руками?",
                            BindingContext = "порезы"
                        },
                        new Button
                        {
                            Text = "Красивый маникюр",
                            BindingContext = "руки"
                        },
                        new Button
                        {
                            Text = "Это настоящие ногти?",
                            BindingContext = "руки"
                        }
                    }
                }
            },
            {
                "руки",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Ух ты, красивые руки.",
                        "Саша прищурившись и с недопониманием посмотрела на меня.",
                        "- Что?",
                        "- [Саша] Немного не в тему...",
                        "- Ну, я старался сделать комплимент.",
                        "- [Саша] Старайся."                      
                    },
                    link = "а что, если"
                }
            },
            { 
                "а что, если",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Прошла затяжная минута. Девушка решила нарушить тишину...",
                        "- [Саша] А что будет, если я не спрыгну?",
                        "- Твой мир поменяется.",
                        "- [Саша] Почему ты так считаешь?",
                        "- После такого *я показал руками нашу обстановку* у каждого он поменяется.",
                        "- [Саша] Но это не так. Мне снова придется вернуться домой и продолжать свое никчемное существование.",
                        "- Ну... Ты можешь и не возвращаться. Ты самоконтролируемый человек, в праве действовать так, как сама этого хочешь.",
                        "- [Саша] Смешно... И где я буду жить? В парках холодно, а с бомжами драться за картонку как то не хочется."                      
                    },
                    buttons = new List<Button>
                    {
                        new Button
                        {
                            Text = "Я помогу с деньгами!",
                            BindingContext = "деньги"
                        },
                        new Button
                        {
                            Text = "Можешь пожить у меня",
                            BindingContext = "дом"
                        },
                        new Button
                        {
                            Text = "Хочешь жить - умей вертеться",
                            BindingContext = "сама давай"
                        }
                    }
                }
            },
            {
                "сама давай",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Есть одна хорошая поговорка - \"Хочешь жить - умей вертеться\".",
                        "- [Саша] Эмм...",
                        "- Что?",
                        "- [Саша] Чего я делать не могу.",
                        "- Да ты сильно не переживай. Все устаканится.",
                        "- [Саша] Мало верится что-то.",
                        "- Ну, я же живу в этом доме. Можешь пожить у меня, если с ночлежкой натяжка.",
                        "- [Саша] Спасибо, я подумаю над этим.",
                        "- Только вот я живу не один.",
                        "- [Саша] М?"
                    },
                    link = "саймон"
                }
            },
            {
                "дом",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Ну, я живу один.",
                        "- [Саша] И?"
                    },
                    link = "саймон"
                }
            },
            {
                "деньги",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Я помогу тебе с деньгами.",
                        "- [Саша] Нет, что ты, не стоит. Я и не возьму.",
                        "- Но они тебе пригодятся.",
                        "- [Саша] Я украла немного денег, когда убегала из дома. На первое время мне хватит.",
                        "- Хорошо, а спать где ты будешь?",
                        "- [Саша] Не знаю...",
                        "- Ну, ты можешь побыть у меня какое то время, я и так живу один.",
                        "- [Саша] А?",
                    },
                    link = "саймон"
                }
            },
            {
                "саймон",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Точнее со мной живет кот.",
                        "- [Саша] О, тебя ждут дома.",
                        "- Да, я его очень люблю. Мы друг друга хорошо понимаем. По вечерам, когда мне грустно, он садится на стол, за которым я работаю, и сидит. Сидит пока я не пойду спать. Он помогает молчанием. Порой он меня выслушивает. Да-да, я общаюсь с котом. А иногда он ложится мне на ноги и начинает мурчать. Этот звук так успокаивает. И вот так мы не редко засыпали вдвоем.",
                        "В конце своего монолога я заметил, что девушка смотрела на меня с милой улыбкой, от которой я даже немного смутился.",
                        "- [Саша] Да, каждому порой нужен такой кот. А как его зовут?",
                        "- Саймон.",
                        "- [Саша] Хах, неплохо."
                    },
                    link = "обреченность"
                }
            },
            {
                "обреченность",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Мы еще немного посидели в тишине. Я посмотрел под ноги. Там, внизу, под нами исход ждали парамедики, пожарные и полиция. Ух, что же нас с Сашей ждет...",
                        "- [Саша] Артем...",
                        "- Да?",
                        "- [Саша] Я не верю в свое будущее, не верю, что у меня что-то получится, что я кому-то нужна. Понимаешь, я сломлена.",
                        "- Начни жизнь с чистого листа.",
                        "- [Саша] А дальше? Почерк то я не поменяю.",
                        "- Может тебе стоит задуматься об изменении своей жизни?",
                        "- [Саша] Например?",
                        "- Ну, первое что нужно сделать, это обрести цель.",
                        "- [Саша] Но как?",
                        "- Я не знаю... Какое твое хобби, что тебе нравится в жизни?",
                        "- [Саша] Ну, я люблю рисовать. Люблю рисовать красивую жизнь, не ту, в которой мы живем.",
                        "- Не хочешь стать художником?",
                        "- [Саша] Ну стану, а дальше?",
                        "- Хм... Не знаю. Будешь картины писать.",
                        "- [Саша] И это ты называешь целью?",
                        "- Понимаешь, без цели нормально жить не получится. У каждого она должна быть. Например у верующих цель есть совершать добро, у докторов это спасать людей, у матерей же вырастить свое дитя и поставить на правильный путь. Порой поиск цели в жизни и есть цель жизни.",
                        "- [Саша] Я тебе говорила, что ты чертов философ?",
                        "- Кажется да.",
                        "- [Саша] Не дружишь ты с памятью. Сладкое не любишь что ли?",
                        "- К-хех, частично."
                    },
                    link = "надежда"
                }
            },
            {
                "надежда",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Девушка уперлась руками и откинула голову. Приближался вечер. Небо окрасил оранжевый закат. Ветер стих, унес за собой тучи, оставив чистое небо. В один миг все стало так красиво.",
                        "- [Саша] Еще я хотела стать психологом.",
                        "- Ох, здорово. Психологи как и врачи спасают жизни людям.",
                        "- [Саша] Да, я хотела помогать таким как я.",
                        "- Что тебе мешает такой стать?",
                        "- [Саша] Мое нынешнее состояние.",
                        "- Оно у тебя не станет прежним. Это хороший толчок поменять свое направление.",
                        "- [Саша] Да, возможно..."
                    },
                    link = "жакет"
                }
            },
            {
                "жакет",
                new PageForm
                {
                    link = "репортаж"
                }
            },
            {
                "репортаж",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "С окон соседнего здания начали снимать репортаж. Девушка с микрофоном что-то говорила на камеру и изредка указывала рукой в нашу сторону.",
                        "- [Саша] Куда ты смотришь?",
                        "- Нас показывают по новостям.",
                        "Девушка приподнялась, уловила взглядом откуда нас снимают и показала в ту сторону средний палец.",
                        "- [Саша] Чтоб у вас там камера по лестнице вниз полетела и вы сами за ней!",
                        "- К-хех, смешно. И жестоко...",
                        "- [Саша] Как и этот мир.",
                        "Девушка обратно легла и смотрела на небо."
                    },
                    link = "страх"
                }
            },
            {
                "страх",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- [Саша] Артем, я так боюсь жить, боюсь сделать неверный шаг. Ведь вдруг что-то станет хуже. Хотя... Сомневаюсь, что может стать хуже, чем сейчас.",
                        "- Жизнь и состоит из постоянных выборов, решений и их последствий.",
                        "- [Саша] Почему все так сложно?",
                        "- Это люди постарались...",
                        "- [Саша] Ненавижу людей.",
                        "- Ну, бывают и хорошие люди, которые помогают другим.",
                        "- [Саша] Ну кроме них...",
                        "- А кроме них уже нелюди.",
                        "- [Саша] Хм, да..."
                    },
                    link = "пора"
                }
            },
            {
                "пора",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Девушка поднялась, посмотрела вниз и произнесла...",
                        "- [Саша] Твое приглашение еще в силе?",
                        "- Да, оставайся сколько хочешь. Будет веселее.",
                        "- [Саша] Хорошо, спасибо большое.",
                        "Девушка тяжело вздохнула со словами",
                        "- [Саша] Очевидно не сегодня...",
                        "- И лучше бы никогда.",
                        "- [Саша] Почему же?",
                        "- Будет жалко, если такая девушка уйдет из этого мира.",
                        "- [Саша] Да кому я нужна, Господи.",
                        "- Ну... Теперь мне.",
                        "- [Саша] Кормить Саймона, пока ты не дома?",
                        "- Вы с ним подружитесь.",
                        "- [Саша] Надеюсь он примет меня. Не хочется быть в плохих отношениях со вторым хозяином дома.",
                        "- Он добрый, все у вас будет хорошо.",
                        "Мы мило разговаривали и внутри себя я был рад, что смог спасти человека. Может вот она, причина, по которой я жил?",
                        "- [Саша] Твоя куртка конечно теплая, но не отопительная батарея.",
                        "- Ой. Намек понят."
                    },
                    link = "спуск"
                }
            },
            {
                "спуск",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Мы встали и направились к выходу, где нас уже ждали. Я взял Сашу за руку и остановил.",
                        "- Послушай. Сейчас нас могу",
                        "- [Саша] *перебивая* Я знаю.",
                        "Я обнял ее напоследок, взял за руки и смотря в ее глубокие глаза, сказал...",
                        "- Жди меня.",
                        "- [Саша] Буду."
                    },
                    link = "ад"
                }
            },
            {
                "ад",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Она первой спустилась по лестнице вниз, где ее сразу схватили, вслед за ней и меня. Понятно, я же ведь вмешался в спасительную операцию, где все делали полное \"ничего\".",
                        "Мы спустились с небес на землю, в ад."
                    },
                    link = "в полицейской машине"
                }
            },
            {
                "в полицейской машине",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Нас рассадили в разные машины. Саша была смиренна, она верила, что я приду. А я верил, что она будет ждать.",
                        "Вскоре машины завелись и мы поехали в разном направлении. По дороге в участок копы пытались вдолбить в меня мораль. Мне о совести кричат бессовестные...",
                        "- [Полицейский] Вы нарушили закон, понимаете? Что за самовольство? А вдруг эта суицидница таки спрыгнула? Тогда вас бы посадили на лет 5 как минимум!",
                        "Я молчал, уставившись в окно. Меня ничуть не интересовали слова копа, я лишь думал о нашем диалоге на крыше, думал о ней. А еще я думал о своем коте.",
                        "Бедолаге придется сегодня голодать. Вот ведь собаки некрещеные, кота моего за что?!",
                        "- [Полицейский] Вы хоть меня слышите? Я вообще то к вам обращаюсь.",
                        "- *зевая* Интересные заявления. Прекрасно вас слушаю, продолжайте.",
                        "И мы оба знали, что это ложь. Полицейский понял меня и оставшийся путь мы проехали под гул мотора.",
                        "Весь оставшийся день я просидел в отделении."
                    },
                    link = "дедлайны"
                }
            },
            {
                "дедлайны",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "На следующий день, заплатив штраф, и не только штраф, я так и вышел из этого помещения. Этот же день я убил на поиски Саши, узнавал в какую больницу ее отвели.",
                        "Дома я побывал лишь чтобы накормить кота и заморозить кое-какие дела, от некоторых вовсе отказался.",
                        "Конечно это пагубно скажется на моем месячном доходе, но не каждый же день спасаешь людей на крышах..."
                    },
                    link = "нашел ее"
                }
            },
            {
                "нашел ее",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "В итоге я нашел где они ее держат, сел на такси и приехал к ней.",
                        "Кое как я уломал доктора хоть на минутку разрешить мне поговорить с ней.",
                        "С большой радостью Саша встретила меня у порога. Я ее крепко обнял и всячески утешал.",
                        "У меня было мало времени рассказать ей дальнейший наш план так что продумав все это, я записал его на бумажке и отдал ей.",
                        "За оставшееся время я спросил у неё..."
                    },
                    link = "ну как ты?"
                }
            },
            {
                "ну как ты?",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- Ну как ты здесь?",
                        "- [Саша] Нормально. Таблетками кормят, антидепрессанты всякие.",
                        "- И как тебе?",
                        "- [Саша] Да пока гору за шкафом не заметили. Уборщицы тут так себе.",
                        "...улыбчиво рассказала мне девушка. Вдруг в дверь постучались.",
                        "- Мне пора уходить.",
                        "- [Саша] *грустно* Понятно. Ну иди.",
                        "- Я еще приду!",
                        "- [Саша] Хах, буду тебя ждать."
                    },
                    link = "док"
                }
            },
            {
                "док",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Потом я повстречался с лечащим доктором и узнал примерную дату выписки.",
                        "Со спокойной душой я поехал домой, где меня дожидался Саймон.",
                        "Весь вечер я говорил с ним, рассказывая события минувших дней..."
                    },
                    link = "душевный разговор"
                }
            },
            {
                "душевный разговор",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "- И вот понимаешь, что теперь нас будет трое. Нужно предоставить ей кровать. Придется спать на полу какое-то время, пока не куплю еще одну.",
                        "Саймон закрыв глаза, слушал меня.",
                        "- Она тебе понравится, она красивая. А насчет себя не переживай, ты ей давно уже понравился. Только вот... Как мы дальше то жить будем?",
                        "Не успев занервничать, кот сразу прыгнул мне на ноги и улегся в клубок.",
                        "- *гладя* Спасибо друг, это всегда помогает."
                    },
                    link = "сон"
                }
            },
           {
                "сон",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Так мы и сидели до поры до времени. Морфей укутал меня сказкой и я провалился в сон.",
                        "Давно я не видел снов однако. Обширная большая равнина. Зеленая трава. Одинокое дерево посреди взора.",
                        "Я чувствовал сильный жар, словно я в центре самого Солнца.",
                        "В целях избежать прямых солнечных лучей я рысью побежал под тень дерева, но это мне не сильно помогло. Жара была невыносимой.",
                        "- [...] А я ждала тебя."
                    },
                    link = "привет"
                }
            },
           {
                "привет",
                new PageForm
                {
                    texts = new List<string>
                    {
                        "Я обернулся и увидел Сашу со стаканом, который она мне протягивала.",
                        "По нему стекали капельки, я взял стакан и понял, что вода в нем очень холодная и залпом выпил содержимое.",
                        "Мне стало в разы лучше, взгляд прояснился. Саша стояла рядом со мной, положив голову на мое плечо."
                    },
                    link = "mainPage"
                }
            },

        };
	}
}