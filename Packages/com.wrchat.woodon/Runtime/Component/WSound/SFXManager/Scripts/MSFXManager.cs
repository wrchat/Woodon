using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MSFXManager : MBase
	{
		[field: Header("_" + nameof(MSFXManager))]
		[field: SerializeField] public AudioClip[] AudioClips { get; private set; }
		[field: SerializeField] public float[] VolumeScaleWhenPlayOneShot { get; private set; }
		[SerializeField] private AudioSource audioSource;

		[Header("_" + nameof(MSFXManager) + " - Options")]
		[SerializeField] private bool stopWhenEvent = true;
		[SerializeField] private bool playOneShot = true;
		[SerializeField] private bool stopWhenStart = true;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (stopWhenStart)
			{
				audioSource.mute = true;
				SendCustomEventDelayedSeconds(nameof(StopSFX), 1f);
				SendCustomEventDelayedSeconds(nameof(UnmuteAudioSource), 2f);
			}
		}

		public void UnmuteAudioSource()
		{
			audioSource.mute = false;
		}

		public void PlaySFX_G(int index)
		{
			MDebugLog($"{nameof(PlaySFX_G)} : {nameof(index)} = {index}");

			SetOwner();
			SendCustomNetworkEvent(NetworkEventTarget.All, $"{nameof(PlaySFX_L)}{index}");
		}

		public void PlaySFX_L(int index)
		{
			MDebugLog($"{nameof(PlaySFX_L)} : {nameof(index)} = {index}");

			bool isInvalidIndex = index >= AudioClips.Length;
			bool isElementNull = isInvalidIndex || AudioClips[index] == null;
			
			if (isInvalidIndex || isElementNull)
				return;

			if (VolumeScaleWhenPlayOneShot.Length > index)
				PlaySFX(AudioClips[index], VolumeScaleWhenPlayOneShot[index]);
			else
				PlaySFX(AudioClips[index]);
		}

		public void StopSFX_Global()
		{
			MDebugLog(nameof(StopSFX_Global));

			SetOwner();
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(StopSFX));
		}

		public void PlaySFX(AudioClip audioClip, float volumeScaleWhenPlayOneShot = 1f)
		{
			if (audioClip == null)
				return;

			MDebugLog($"{nameof(PlaySFX)} : {nameof(audioClip)} = {audioClip.name}");

			if (stopWhenEvent)
			{
				audioSource.Stop();
			}

			if (playOneShot)
			{
				audioSource.PlayOneShot(audioClip, volumeScaleWhenPlayOneShot);
			}
			else
			{
				audioSource.clip = audioClip;
				audioSource.Play();
			}
		}

		public void StopSFX()
		{
			MDebugLog(nameof(StopSFX));
			audioSource.Stop();
		}

		#region HorribleEvents
		[ContextMenu(nameof(PlaySFX_G0))]
		public void PlaySFX_G0() => PlaySFX_G(0);
		public void PlaySFX_G1() => PlaySFX_G(1);
		public void PlaySFX_G2() => PlaySFX_G(2);
		public void PlaySFX_G3() => PlaySFX_G(3);
		public void PlaySFX_G4() => PlaySFX_G(4);
		public void PlaySFX_G5() => PlaySFX_G(5);
		public void PlaySFX_G6() => PlaySFX_G(6);
		public void PlaySFX_G7() => PlaySFX_G(7);
		public void PlaySFX_G8() => PlaySFX_G(8);
		public void PlaySFX_G9() => PlaySFX_G(9);
		public void PlaySFX_G10() => PlaySFX_G(10);
		public void PlaySFX_G11() => PlaySFX_G(11);
		public void PlaySFX_G12() => PlaySFX_G(12);
		public void PlaySFX_G13() => PlaySFX_G(13);
		public void PlaySFX_G14() => PlaySFX_G(14);
		public void PlaySFX_G15() => PlaySFX_G(15);
		public void PlaySFX_G16() => PlaySFX_G(16);
		public void PlaySFX_G17() => PlaySFX_G(17);
		public void PlaySFX_G18() => PlaySFX_G(18);
		public void PlaySFX_G19() => PlaySFX_G(19);
		public void PlaySFX_G20() => PlaySFX_G(20);
		public void PlaySFX_G21() => PlaySFX_G(21);
		public void PlaySFX_G22() => PlaySFX_G(22);
		public void PlaySFX_G23() => PlaySFX_G(23);
		public void PlaySFX_G24() => PlaySFX_G(24);
		public void PlaySFX_G25() => PlaySFX_G(25);
		public void PlaySFX_G26() => PlaySFX_G(26);
		public void PlaySFX_G27() => PlaySFX_G(27);
		public void PlaySFX_G28() => PlaySFX_G(28);
		public void PlaySFX_G29() => PlaySFX_G(29);
		public void PlaySFX_G30() => PlaySFX_G(30);
		public void PlaySFX_G31() => PlaySFX_G(31);
		public void PlaySFX_G32() => PlaySFX_G(32);
		public void PlaySFX_G33() => PlaySFX_G(33);
		public void PlaySFX_G34() => PlaySFX_G(34);
		public void PlaySFX_G35() => PlaySFX_G(35);
		public void PlaySFX_G36() => PlaySFX_G(36);
		public void PlaySFX_G37() => PlaySFX_G(37);
		public void PlaySFX_G38() => PlaySFX_G(38);
		public void PlaySFX_G39() => PlaySFX_G(39);
		public void PlaySFX_G40() => PlaySFX_G(40);
		public void PlaySFX_G41() => PlaySFX_G(41);
		public void PlaySFX_G42() => PlaySFX_G(42);
		public void PlaySFX_G43() => PlaySFX_G(43);
		public void PlaySFX_G44() => PlaySFX_G(44);
		public void PlaySFX_G45() => PlaySFX_G(45);
		public void PlaySFX_G46() => PlaySFX_G(46);
		public void PlaySFX_G47() => PlaySFX_G(47);
		public void PlaySFX_G48() => PlaySFX_G(48);
		public void PlaySFX_G49() => PlaySFX_G(49);
		public void PlaySFX_G50() => PlaySFX_G(50);
		public void PlaySFX_G51() => PlaySFX_G(51);
		public void PlaySFX_G52() => PlaySFX_G(52);
		public void PlaySFX_G53() => PlaySFX_G(53);
		public void PlaySFX_G54() => PlaySFX_G(54);
		public void PlaySFX_G55() => PlaySFX_G(55);
		public void PlaySFX_G56() => PlaySFX_G(56);
		public void PlaySFX_G57() => PlaySFX_G(57);
		public void PlaySFX_G58() => PlaySFX_G(58);
		public void PlaySFX_G59() => PlaySFX_G(59);
		public void PlaySFX_G60() => PlaySFX_G(60);
		public void PlaySFX_G61() => PlaySFX_G(61);
		public void PlaySFX_G62() => PlaySFX_G(62);
		public void PlaySFX_G63() => PlaySFX_G(63);
		public void PlaySFX_G64() => PlaySFX_G(64);
		public void PlaySFX_G65() => PlaySFX_G(65);
		public void PlaySFX_G66() => PlaySFX_G(66);
		public void PlaySFX_G67() => PlaySFX_G(67);
		public void PlaySFX_G68() => PlaySFX_G(68);
		public void PlaySFX_G69() => PlaySFX_G(69);
		public void PlaySFX_G70() => PlaySFX_G(70);
		public void PlaySFX_G71() => PlaySFX_G(71);
		public void PlaySFX_G72() => PlaySFX_G(72);
		public void PlaySFX_G73() => PlaySFX_G(73);
		public void PlaySFX_G74() => PlaySFX_G(74);
		public void PlaySFX_G75() => PlaySFX_G(75);
		public void PlaySFX_G76() => PlaySFX_G(76);
		public void PlaySFX_G77() => PlaySFX_G(77);
		public void PlaySFX_G78() => PlaySFX_G(78);
		public void PlaySFX_G79() => PlaySFX_G(79);
		public void PlaySFX_G80() => PlaySFX_G(80);
		public void PlaySFX_G81() => PlaySFX_G(81);
		public void PlaySFX_G82() => PlaySFX_G(82);
		public void PlaySFX_G83() => PlaySFX_G(83);
		public void PlaySFX_G84() => PlaySFX_G(84);
		public void PlaySFX_G85() => PlaySFX_G(85);
		public void PlaySFX_G86() => PlaySFX_G(86);
		public void PlaySFX_G87() => PlaySFX_G(87);
		public void PlaySFX_G88() => PlaySFX_G(88);
		public void PlaySFX_G89() => PlaySFX_G(89);
		public void PlaySFX_G90() => PlaySFX_G(90);
		public void PlaySFX_G91() => PlaySFX_G(91);
		public void PlaySFX_G92() => PlaySFX_G(92);
		public void PlaySFX_G93() => PlaySFX_G(93);
		public void PlaySFX_G94() => PlaySFX_G(94);
		public void PlaySFX_G95() => PlaySFX_G(95);
		public void PlaySFX_G96() => PlaySFX_G(96);
		public void PlaySFX_G97() => PlaySFX_G(97);
		public void PlaySFX_G98() => PlaySFX_G(98);
		public void PlaySFX_G99() => PlaySFX_G(99);
		public void PlaySFX_G100() => PlaySFX_G(100);

		[ContextMenu(nameof(PlaySFX_L0))]
		public void PlaySFX_L0() => PlaySFX_L(0);
		public void PlaySFX_L1() => PlaySFX_L(1);
		public void PlaySFX_L2() => PlaySFX_L(2);
		public void PlaySFX_L3() => PlaySFX_L(3);
		public void PlaySFX_L4() => PlaySFX_L(4);
		public void PlaySFX_L5() => PlaySFX_L(5);
		public void PlaySFX_L6() => PlaySFX_L(6);
		public void PlaySFX_L7() => PlaySFX_L(7);
		public void PlaySFX_L8() => PlaySFX_L(8);
		public void PlaySFX_L9() => PlaySFX_L(9);
		public void PlaySFX_L10() => PlaySFX_L(10);
		public void PlaySFX_L11() => PlaySFX_L(11);
		public void PlaySFX_L12() => PlaySFX_L(12);
		public void PlaySFX_L13() => PlaySFX_L(13);
		public void PlaySFX_L14() => PlaySFX_L(14);
		public void PlaySFX_L15() => PlaySFX_L(15);
		public void PlaySFX_L16() => PlaySFX_L(16);
		public void PlaySFX_L17() => PlaySFX_L(17);
		public void PlaySFX_L18() => PlaySFX_L(18);
		public void PlaySFX_L19() => PlaySFX_L(19);
		public void PlaySFX_L20() => PlaySFX_L(20);
		public void PlaySFX_L21() => PlaySFX_L(21);
		public void PlaySFX_L22() => PlaySFX_L(22);
		public void PlaySFX_L23() => PlaySFX_L(23);
		public void PlaySFX_L24() => PlaySFX_L(24);
		public void PlaySFX_L25() => PlaySFX_L(25);
		public void PlaySFX_L26() => PlaySFX_L(26);
		public void PlaySFX_L27() => PlaySFX_L(27);
		public void PlaySFX_L28() => PlaySFX_L(28);
		public void PlaySFX_L29() => PlaySFX_L(29);
		public void PlaySFX_L30() => PlaySFX_L(30);
		public void PlaySFX_L31() => PlaySFX_L(31);
		public void PlaySFX_L32() => PlaySFX_L(32);
		public void PlaySFX_L33() => PlaySFX_L(33);
		public void PlaySFX_L34() => PlaySFX_L(34);
		public void PlaySFX_L35() => PlaySFX_L(35);
		public void PlaySFX_L36() => PlaySFX_L(36);
		public void PlaySFX_L37() => PlaySFX_L(37);
		public void PlaySFX_L38() => PlaySFX_L(38);
		public void PlaySFX_L39() => PlaySFX_L(39);
		public void PlaySFX_L40() => PlaySFX_L(40);
		public void PlaySFX_L41() => PlaySFX_L(41);
		public void PlaySFX_L42() => PlaySFX_L(42);
		public void PlaySFX_L43() => PlaySFX_L(43);
		public void PlaySFX_L44() => PlaySFX_L(44);
		public void PlaySFX_L45() => PlaySFX_L(45);
		public void PlaySFX_L46() => PlaySFX_L(46);
		public void PlaySFX_L47() => PlaySFX_L(47);
		public void PlaySFX_L48() => PlaySFX_L(48);
		public void PlaySFX_L49() => PlaySFX_L(49);
		public void PlaySFX_L50() => PlaySFX_L(50);
		public void PlaySFX_L51() => PlaySFX_L(51);
		public void PlaySFX_L52() => PlaySFX_L(52);
		public void PlaySFX_L53() => PlaySFX_L(53);
		public void PlaySFX_L54() => PlaySFX_L(54);
		public void PlaySFX_L55() => PlaySFX_L(55);
		public void PlaySFX_L56() => PlaySFX_L(56);
		public void PlaySFX_L57() => PlaySFX_L(57);
		public void PlaySFX_L58() => PlaySFX_L(58);
		public void PlaySFX_L59() => PlaySFX_L(59);
		public void PlaySFX_L60() => PlaySFX_L(60);
		public void PlaySFX_L61() => PlaySFX_L(61);
		public void PlaySFX_L62() => PlaySFX_L(62);
		public void PlaySFX_L63() => PlaySFX_L(63);
		public void PlaySFX_L64() => PlaySFX_L(64);
		public void PlaySFX_L65() => PlaySFX_L(65);
		public void PlaySFX_L66() => PlaySFX_L(66);
		public void PlaySFX_L67() => PlaySFX_L(67);
		public void PlaySFX_L68() => PlaySFX_L(68);
		public void PlaySFX_L69() => PlaySFX_L(69);
		public void PlaySFX_L70() => PlaySFX_L(70);
		public void PlaySFX_L71() => PlaySFX_L(71);
		public void PlaySFX_L72() => PlaySFX_L(72);
		public void PlaySFX_L73() => PlaySFX_L(73);
		public void PlaySFX_L74() => PlaySFX_L(74);
		public void PlaySFX_L75() => PlaySFX_L(75);
		public void PlaySFX_L76() => PlaySFX_L(76);
		public void PlaySFX_L77() => PlaySFX_L(77);
		public void PlaySFX_L78() => PlaySFX_L(78);
		public void PlaySFX_L79() => PlaySFX_L(79);
		public void PlaySFX_L80() => PlaySFX_L(80);
		public void PlaySFX_L81() => PlaySFX_L(81);
		public void PlaySFX_L82() => PlaySFX_L(82);
		public void PlaySFX_L83() => PlaySFX_L(83);
		public void PlaySFX_L84() => PlaySFX_L(84);
		public void PlaySFX_L85() => PlaySFX_L(85);
		public void PlaySFX_L86() => PlaySFX_L(86);
		public void PlaySFX_L87() => PlaySFX_L(87);
		public void PlaySFX_L88() => PlaySFX_L(88);
		public void PlaySFX_L89() => PlaySFX_L(89);
		public void PlaySFX_L90() => PlaySFX_L(90);
		public void PlaySFX_L91() => PlaySFX_L(91);
		public void PlaySFX_L92() => PlaySFX_L(92);
		public void PlaySFX_L93() => PlaySFX_L(93);
		public void PlaySFX_L94() => PlaySFX_L(94);
		public void PlaySFX_L95() => PlaySFX_L(95);
		public void PlaySFX_L96() => PlaySFX_L(96);
		public void PlaySFX_L97() => PlaySFX_L(97);
		public void PlaySFX_L98() => PlaySFX_L(98);
		public void PlaySFX_L99() => PlaySFX_L(99);
		public void PlaySFX_L100() => PlaySFX_L(100);
		#endregion
	}
}