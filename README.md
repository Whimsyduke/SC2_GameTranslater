# SC2_GameTranslate
Hi SC2 modders, I'm Whimsyduke form Team Froggy&Catty. SC2_GameTranslater is one of our open source projects for Starcraft II mod community. With this tool, Modders can translate your Map/Mod in to different local languages, this tool also support various fuctions such as collation, content filtration and in game preview. We wish this tool can help you sharing your works and increasing communications between people all around the world. 
Good Game and Good Luck!



星际争霸2诸位mod作者，大家好。我是来自喵喵呱呱团队的孔明。星际争霸2游戏翻译器是我们为星际2社区开发的众多开源项目之一。该工具能够帮助作者将自己的MOD翻译为多种语言。我们希望该工具能够帮助全世界的星际2MOD作者以及游戏玩家。通过翻译的方式促进作品的推广和交流。
在开发过程中我们结合自己的经验，制作了许多实用功能。该工具支持备份，修改，文本比较，并且支持同语种校对、文本筛选、游戏内效果预览等功能。你甚至不需要使用完整的MOD或地图文件就可以进行翻译工作。因此该工具也适合玩家或项目合作者独立工作使用。
GG&GL!

使用说明
=======

![image](https://github.com/Whimsyduke/SC2_GameTranslater/blob/master/HowtoUse/Tool02.png)

1.文件
------------

新建
--------

>>新建一个翻译项目，软件会要求你选择以组件文件夹形式保存的地图文件，双击ComponentList.SC2Components打开即可。使用组件文件夹可以加快地图的读取速度，当应用翻译结果时，对应的翻译文本会自动保存到enUS.SC2Data等本地化文件夹中。

打开
--------

>>打开一个保存的项目文件，当你的项目曾经保存过，会生成一个SC2Gametran文件，该文件保存有翻译所需的全部信息。因此你可以在无需地图完整文件的情况下继续翻译工作。

保存
--------

>>将你的翻译修改保存为SC2Gametran文件。

另存为
--------

>>将你的翻译修改保存为一个新的SC2Gametran文件。

关闭
--------

>>关闭软件。

应用翻译
--------

将你的翻译内容覆盖到组件文件夹中，这会同时修改游戏中的文本。

重载翻译
--------

从另外一个项目文件中将翻译内容导入至该项目。修改你的翻译内容，你可以选择导入全部或部分语言。

重载文本
--------

从另外一个项目文件中将翻译内容导入至该项目，但不会修改翻译内容，而是进行文本对比，方便你查看哪些文本的翻译发生了变化。（默认隐藏，需要在主翻译区勾选相关选项）

2.筛选记录
--------------

上一纪录/下一记录
------------

当使用筛选或搜索工具后，主翻译区的文本显示会发生变化，使用该工具可以切换显示状态。

3.翻译语言
-----------

选择你本次翻译的原始语言和目标语言，如enUS翻译至zhCN。当你的原始语言和目标语言相同时，你可以对翻译文本进行修改和校对。


4.搜索
-------------

使用搜索工具能够筛选显示你需要的特定内容。本工具的搜索工具提供搜索范围，大小写匹配和表达式搜索等使用工具。

5过滤器
-------------

所属Galaxy文件
--------------

筛选显示来自不同Galaxy文件的文本。如LibLIB1, LibIB1h等。

所属文本文件
---------------

筛选显示来自游戏、数据、触发器的文本。

文本状态
--------------

筛选显示已修改和未修改的文本。这些信息也会在翻译区用颜色进行标记和区分。

使用状态
--------------

筛选显示游戏中文本的实际使用状态，如新增文本，修改文本，删除文本等，需要与`重载文本功能`配合使用。














Tools for translating text in maps or mods.

### CI Status
| Branch | Status |
| ------ | ------- |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/pra1v3b65rifuwol/branch/master?svg=true)](https://ci.appveyor.com/project/Whimsyduke/sc2-gametranslater/branch/master) |

## Licence
[![LICENSE](https://img.shields.io/badge/license-NPL%20(The%20996%20Prohibited%20License)-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

## Link
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)
