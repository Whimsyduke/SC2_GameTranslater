SC2_GameTranslater
=====================
![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Froggy&Catty_Logo.png)

### CI Status
| Branch | Status |
| ------ | ------- |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/pra1v3b65rifuwol/branch/master?svg=true)](https://ci.appveyor.com/project/Whimsyduke/sc2-gametranslater/branch/master) |

## Licence
[![LICENSE](https://img.shields.io/badge/license-NPL%20(The%20996%20Prohibited%20License)-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

## Link
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)



Hi SC2 modders, I'm Whimsyduke form Team Froggy&Catty. SC2_GameTranslater is one of our open source projects for Starcraft II mod community. With this tool, Modders can translate your Map/Mod in to different local languages, we wish this tool can help you sharing your works and increasing communications between people all around the world. 

Based on our translate experience, we have developed utiilty kits such as auto backup, modification and text search/comparison to simplify and optimize the translation process. This tool also support various fuctions such as collation, content filtration and in game preview. The translater can even finish the translation without complete mod or map files. So this tool can also be good partner of players and project cooperators.

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/23.png)

How to use
=======

Before We Start
============

>If you first open this software and find there are so many Chinese words, don't be panic. Just follow the picture to find the language selection menu. Then all things will be familiar.

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/English.png)

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/EnglishSet.png)



Toolbar
============

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool01.png)

1.File
------------

New
--------

>Create a new translation project for SC2. The tool requires a map or mod that saved as SC2component folds. You can double click the ComponentList.SC2Components to open this mod/map. Using SC2component fold can increase the loading speed of your mod/map, once you apply your translation, the files will automaticly save your translated text to local language folds like enUS.SC2Data.

Open
--------

>Open a saved project file. If you once saved your work, a SC2Gametran file will be created, it contains all necessary information that translation needed. So you can continue your work or share it will partners without the Map/mod files.

Save
--------

>Save your works to SC2Gametran file.

Save as
--------

>Save your works to a new SC2Gametran file.

Close
--------

>Shutdown the software.

Apply project
--------

>Send your translated text to the Mod/map (SC2component folds). It will modify the ingame text as well.


Reload Translation
--------

>Import translated text from another SC2Gametran file. It will also refresh your translated text. You can select one or more languages you need.

Reload Compontends
--------

>Import translated text from old version SC2Gametran file or Mod/map (SC2component folds). It won't change your text, instead, Abandoned text and usage status will be refreshed, you can compare these text with current text (source text and edited text) so that identify the changed text during the updates. (the Abandoned text and usage status are hide by default, you may need to set them by right click the table title of main translation zone.)


2.Record Filter
--------------

Last Record/Next Record
------------

>After using the filter or search kits, the content in main translation zone will be changed. You can use this kit to shift between different statuses just like page up/down.

3.Language
-----------

>Select the languages that your reference and target of translation. Such as enUS to zhCN. You can modify and collate your text as well if you use same reference and target languages.

4.Search
-------------

>Search tool allow you to filter and find the contents you need. You can search base on different location, match case or Regular expression.

5.Filter
-------------

Galaxy File Filter
--------------

>Filt text from different galaxy files, such as LibLIB1, LibIB1h and so on.

Record File Filter
---------------

>Filt text from different record, such as data editor, trigger and so on.

Text Status Filter
--------------

>Filt text from different status, such as modified and unmodified and so on.

Usage Status Filter
--------------

>Filt text base on the usage status in the mod. Such as added, modified and abandoned. Requires using `Reload Compontends` first.




Text overlap tool
-------------

This tool can overlap the translated text to other local languages. Such as using enUS in frFR and deDE.
![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool07.png)

1.Target language
-----------

>Select one or more languages that you want overlap to.

2.Source language
-----------

>Select one language as the source language, make sure it's already translated.

3.Apply
-------------

>Apply the process then the target languages will be overlaped.


Main Translation Zone
==================

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool03.png)

1.Data Index
-----------

>The unique index of the text.

2.Text ID
--------------

>The text ID in SC2 editor.

3.File
--------------

>The file that contains this text.

4.Text Status
------------

>Show the modify status of the text.

5.Usage Status
------------

>Show the usage status in the mod. Such as added, modified and abandoned. Requires using `Reload Compontends` first.

6.Abandoned Text
--------------

>The default abandoned text is same as the source text. If use the `Relaod Compontends` to load an old version mod, it will show the text of `old version mod` and can be used to compare with current text.

Detail View
==================

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool05.png)

1.Show Language
-------------

>Show all languages that your mod used, it can be set in the SC2 editor, you can see in the picture, there are three activatied languages in the example mod.

2.Game Text Details
------------------

>Show the text in all activatied languages, notice that you can only preview the text here, this tool doesn't support modification.

3.Edited Text
---------------

>Same as the edited text frame in the main translation zone, but bigger and easier to read and write.



4.Text Combine in Galaxy
--------------------

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool09.png)
>Show the text details in the galaxy script, if a sentence contains various strings in galaxy script, you can review the final result here too, such as `this weapon creates`+`points damages.`.


>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


星际争霸2游戏翻译器
=================

星际争霸2诸位mod作者，大家好。我是来自喵喵呱呱团队的孔明。星际争霸2游戏翻译器是我们为星际2社区开发的众多开源项目之一。该工具能够帮助作者将自己的MOD翻译为多种语言。我们希望该工具能够帮助全世界的星际2MOD作者以及游戏玩家。通过翻译的方式促进作品的推广和交流。

在开发过程中我们结合自己的经验，制作了许多实用功能。该工具支持备份，修改，文本比较，并且支持同语种校对、文本筛选、游戏内效果预览等功能。你甚至不需要使用完整的MOD或地图文件就可以进行翻译工作。因此该工具也适合玩家或项目合作者独立工作使用。
GG&GL!

使用说明
=======


工具栏
============

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool02.png)

1.文件
------------

新建
--------

>新建一个翻译项目，软件会要求你选择以组件文件夹形式保存的地图文件，双击ComponentList.SC2Components打开即可。使用组件文件夹可以加快地图的读取速度，当应用翻译结果时，对应的翻译文本会自动保存到enUS.SC2Data等本地化文件夹中。

打开
--------

>打开一个保存的项目文件，当你的项目曾经保存过，会生成一个SC2Gametran文件，该文件保存有翻译所需的全部信息。因此你可以在无需地图完整文件的情况下继续翻译工作。

保存
--------

>将你的翻译修改保存为SC2Gametran文件。

另存为
--------

>将你的翻译修改保存为一个新的SC2Gametran文件。

关闭
--------

>关闭软件。

应用翻译
--------

>将你的翻译内容覆盖到组件文件夹中，这会同时修改游戏中的文本。

重载翻译
--------

>从另外一个项目文件中将翻译内容导入至该项目。修改你的翻译内容，你可以选择导入全部或部分语言。

重载文本
--------

>从另外一个项目文件中将翻译内容导入至该项目，但不会修改翻译内容，而是进行文本对比，方便你查看哪些文本的翻译发生了变化。（默认隐藏，需要在主翻译区勾选相关选项）

2.筛选记录
--------------

上一纪录/下一记录
------------

>当使用筛选或搜索工具后，主翻译区的文本显示会发生变化，使用该工具可以切换显示状态。

3.翻译语言
-----------

>选择你本次翻译的原始语言和目标语言，如enUS翻译至zhCN。当你的原始语言和目标语言相同时，你可以对翻译文本进行修改和校对。


4.搜索
-------------

>使用搜索工具能够筛选显示你需要的特定内容。搜索设置包含搜索范围，大小写匹配和表达式搜索等实用工具。

5.过滤器
-------------

所属Galaxy文件
--------------

>筛选显示来自不同Galaxy文件的文本。如LibLIB1, LibIB1h等。

所属文本文件
---------------

>筛选显示来自游戏、数据、触发器的文本。

文本状态
--------------

>筛选显示已修改和未修改的文本。这些信息也会在翻译区用颜色进行标记和区分。

使用状态
--------------

>筛选显示游戏中文本的实际使用状态，如新增文本，修改文本，删除文本等，需要与`重载文本`功能配合使用。




翻译复制工具
-------------

翻译复制工具用于将已经翻译好的语言文本覆盖到其他语言文本中。比如在法语或德语文本中使用英文文本。
![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool08.png)

1.目标语言
-----------

>在下拉框中选择一种或多种目标语言，这些语言的文本将被覆盖。

2.来源语言
-----------

>选择一种来源语言作为翻译覆盖的来源。

3.开始复制
-------------

>点击该按钮将文本覆盖到选定的区域语言文本中。


翻译区
==================

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool04.png)

1.数据编号
-----------

>文本的唯一序号。

2.文本ID
--------------

>文本在编辑器中的ID。

3.所在文件
--------------

>标记文本的来源。

4.文本状态
------------

>显示文本的修改状态。

5.使用状态
------------

>显示文本在游戏中的使用状态，右击翻译区表格栏可以勾选是否显示。

6.舍弃文本
--------------

>默认和原始文本保持一致。可以使用`重载文本`进行加载，此时将显示`旧版本mod`的文本信息，用于和当前文档进行比较。

细节视图
==================

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool06.png)

1.显示语言
-------------

>显示游戏中激活的区域语言。比如示意图中显示本mod包含英语、韩语和中文三种语言。

2.游戏文本数据
------------------

>显示在所有区域语言下文本的内容，注意此处仅支持文本的查看，不支持修改功能。

3.修改文本
---------------

>与翻译区的修改文本文本框功能相同，提供更大的空间，方便较长文本的翻译和阅读。




4.Galaxy拼接结果
--------------------

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool10.png)
>显示文本在Galaxy脚本中的详细信息，当一个句子由多个字符串组成时，也可以通过该工具查看完整的拼接文本。如“`造成`+`点伤害`”。
