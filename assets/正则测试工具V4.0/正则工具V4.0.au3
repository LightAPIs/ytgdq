#include <Constants.au3>
#include <GuiButton.au3>
#include <GuiImageList.au3>
#include <GUIConstantsEx.au3>
#include <WindowsConstants.au3>
Opt("TrayMenuMode", 1)
Opt("TrayOnEventMode", 1)

Local $aParts[3] = [320, 460, 600], $hWndb, $sTafa = True, $sTafb = True
FileInstall("D:\TDDOWNLOAD\Reg-icos\BG.ico", @TempDir & "\BG.ico", 1)
FileInstall("D:\TDDOWNLOAD\Reg-icos\FDJ.ico", @TempDir & "\FDJ.ico", 1)
FileInstall("D:\TDDOWNLOAD\������ʽ�����ĵ�.chm", @TempDir & "\������ʽ�����ĵ�.chm", 1)

$Form1 = GUICreate("������ʽ���Թ���V4.0        By��ˮľ��", 600, 500)
TraySetOnEvent($TRAY_EVENT_PRIMARYUP, "SpecialEvent")

$Tab1 = GUICtrlCreateTab(2, 2, 596, 475)
GUICtrlSetFont(-1, 10, 400, 0, "Arial")
GUICtrlSetResizing(-1, $GUI_DOCKWIDTH + $GUI_DOCKHEIGHT)

$TabSheet1 = GUICtrlCreateTabItem("ƥ��ģʽ")
GUICtrlSetImage(-1, @TempDir & '\FDJ.ico')
GUICtrlCreateLabel("��Ҫƥ����ַ�����", 10, 40, 110, 17)
$Edit01 = GUICtrlCreateEdit("", 10, 60, 580, 180)
GUICtrlSetLimit(-1, 999999999)

GUICtrlCreateLabel("���ʽ��", 10, 263, 50, 17)
$Input01 = GUICtrlCreateInput("", 60, 260, 400, 21)
$But01 = GUICtrlCreateButton("ƥ��", 460, 258, 60, 25, $WS_GROUP)
_SetIcon(-1, @TempDir & '\FDJ.ico')

$But02 = GUICtrlCreateButton("���", 530, 258, 60, 25, $WS_GROUP)
GUICtrlSetTip(-1, '��յ�ǰģʽ�����пؼ��е����ݡ�')
_SetIcon(-1, 'shell32.dll', 32)

GUICtrlCreateLabel("ƥ������", 10, 300, 60, 17)
$Edit02 = GUICtrlCreateEdit("", 10, 320, 580, 150)
GUICtrlSetLimit(-1, 999999999)

$TabSheet2 = GUICtrlCreateTabItem("�滻ģʽ")
GUICtrlSetImage(-1, @TempDir & '\BG.ico')
GUICtrlCreateLabel("��Ҫ�滻���ַ�����", 10, 40, 110, 17)
$Edit11 = GUICtrlCreateEdit("", 10, 60, 580, 180)
GUICtrlSetLimit(-1, 999999999)

GUICtrlCreateLabel("���ʽ��", 22, 253, 50, 17)
$Input11 = GUICtrlCreateInput("", 70, 250, 400, 21)
$But11 = GUICtrlCreateButton("�滻", 470, 248, 60, 25, $WS_GROUP)
_SetIcon(-1, @TempDir & '\BG.ico')

$But12 = GUICtrlCreateButton("���", 530, 248, 60, 25, $WS_GROUP)
GUICtrlSetTip(-1, '��յ�ǰģʽ�����пؼ��е����ݡ�')
_SetIcon(-1, 'shell32.dll', 32)

GUICtrlCreateLabel("�滻���ݣ�", 10, 283, 60, 17)
$Input12 = GUICtrlCreateInput("", 70, 280, 400, 21)

GUICtrlCreateLabel("�滻������", 480, 283, 60, 17)
$Input13 = GUICtrlCreateCombo("", 540, 280, 50, 21)
GUICtrlSetData(-1, "0|1|2|3|4|5|6|7|8|9|10", '0')
GUICtrlSetTip(-1, 'ƥ���������Ҫִ���滻�Ĵ�����' & @CRLF & 'Ĭ��Ϊ 0 ʹ�� 0 Ϊȫ���滻��')

GUICtrlCreateLabel("�滻�����", 10, 305, 60, 17)
$Edit12 = GUICtrlCreateEdit("", 10, 320, 580, 150)
GUICtrlSetLimit(-1, 999999999)
GUICtrlCreateTabItem("")

GUICtrlCreateLabel("AutoIt3 ������̳��", 340, 5, 110, 17)
$Label0 = GUICtrlCreateLabel("http://www.autoitx.com", 448, 5, 132, 17)
GUICtrlSetCursor(-1, 0)

GUICtrlCreateLabel("����״̬��", 5, 482, 250, 17)
$Label1 = GUICtrlCreateLabel("׼������", 65, 482, 250, 17)
$Label2 = GUICtrlCreateLabel("��������ʽ���Ž̡̳�", 310, 482, 132, 17)
GUICtrlSetCursor(-1, 0)
$Label3 = GUICtrlCreateLabel("��������ʽ�����ĵ���", 460, 482, 132, 17)
GUICtrlSetCursor(-1, 0)
GUISetState(@SW_SHOW)

While 1
	$Pos = GUIGetCursorInfo($Form1)
	If Not @error Then _Hyperlink($Pos[4])

	$nMsg = GUIGetMsg()
	Switch $nMsg
		Case - 3
			Exit
		Case $But01
			Match(GUICtrlRead($Edit01), GUICtrlRead($Input01))
		Case $But11
			Replace(GUICtrlRead($Edit11), GUICtrlRead($Input11), GUICtrlRead($Input12), GUICtrlRead($Input13))
		Case $But02
			GUICtrlSetData($Edit01, '')
			GUICtrlSetData($Edit02, '')
			GUICtrlSetData($Input01, '')
			GUICtrlSetData($Label1, '׼������')
			GUICtrlSetColor($Label1, 0x000000)
			GUICtrlSetState($Edit01, $GUI_FOCUS)
		Case $But12
			GUICtrlSetData($Edit11, '')
			GUICtrlSetData($Edit12, '')
			GUICtrlSetData($Input11, '')
			GUICtrlSetData($Input12, '')
			GUICtrlSetData($Input13, 0)
			GUICtrlSetData($Label1, '׼������')
			GUICtrlSetColor($Label1, 0x000000)
			GUICtrlSetState($Edit11, $GUI_FOCUS)
		Case $Label0
			ShellExecute('http://www.AutoItx.com')
		Case $Label2
			ShellExecute('http://deerchao.net/tutorials/regex/regex.htm')
		Case $Label3
			ShellExecute(@TempDir & "\������ʽ�����ĵ�.chm")
	EndSwitch
WEnd

Func _Hyperlink($hWnda)
	If $hWnda = 27 Or $hWnda = 30 Or $hWnda = 31 Then
		If $sTafb = True Then
			GUICtrlSetFont($hWnda, 9, 400, 4)
			GUICtrlSetColor($hWnda, 0x0080CC)
			$hWndb = $hWnda
			$sTafb = False
			$sTafa = False
		EndIf
	Else
		If $sTafa = False Then
			GUICtrlSetFont($hWndb, 9)
			GUICtrlSetColor($hWndb, 0x000000)
			$sTafa = True
			$sTafb = True
		EndIf
	EndIf
EndFunc   ;==>_Hyperlink

Func Match($oString, $Expressions)
	Local $sResults
	If $oString <> '' And $Expressions <> '' Then
		GUICtrlSetData($Edit02, '')
		$sReg = StringRegExp($oString, $Expressions, 3)
		If UBound($sReg) = 0 Then
			GUICtrlSetData($Label1, 'ƥ��ʧ�ܣ�û���ҵ�ƥ������ݡ�')
			GUICtrlSetColor($Label1, 0xFF0000)
		Else
			For $i = 0 To UBound($sReg) - 1
				$sResults &= StringFormat("[%02d]", $i + 1) & $sReg[$i] & @CRLF
			Next
			GUICtrlSetData($Edit02, $sResults)
			GUICtrlSetData($Label1, 'ƥ��ɹ������ҵ�' & UBound($sReg) & '��ƥ������ݡ�')
			GUICtrlSetColor($Label1, 0x000000)
		EndIf
	Else
		GUICtrlSetData($Label1, '��������ȷ����������ݡ�')
		GUICtrlSetColor($Label1, 0xFF0000)
	EndIf
EndFunc   ;==>Match

Func Replace($oReplace, $Pattern, $Replace, $Count)
	If $oReplace <> '' And $Pattern <> '' Then
		GUICtrlSetData($Edit12, '')
		$Replace = StringRegExpReplace($oReplace, $Pattern, $Replace, $Count)
		If $Replace = $oReplace Then
			GUICtrlSetData($Label1, '�滻ʧ�ܣ�û���ҵ������滻�����ݡ�')
			GUICtrlSetColor($Label1, 0xFF0000)
		Else
			GUICtrlSetData($Edit12, $Replace)
			GUICtrlSetData($Label1, '�滻�ɹ�����鿴�滻�����')
			GUICtrlSetColor($Label1, 0x000000)
		EndIf
	Else
		GUICtrlSetData($Label1, '��������ȷ����������ݡ�')
		GUICtrlSetColor($Label1, 0xFF0000)
	EndIf
EndFunc   ;==>Replace

Func SpecialEvent()
	If WinGetState($Form1) = 7 Then
		GUISetState(@SW_MINIMIZE)
	Else
		GUISetState(@SW_RESTORE)
	EndIf
EndFunc   ;==>SpecialEvent

Func _SetIcon($hWnda, $sFile, $iIndex = 0)
	$hImage1 = _GUIImageList_Create(20, 20, 5, 1, 0)
	_GUIImageList_AddIcon($hImage1, $sFile, $iIndex, True)
	_GUICtrlButton_SetImageList($hWnda, $hImage1)
EndFunc   ;==>_SetIcon