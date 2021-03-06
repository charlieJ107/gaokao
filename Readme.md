# 环己来高考志愿查询系统

网址: https://gaokao.vankyle.cn

嗨, 大家好

我是老蒋, 环己来的创始人. 

一年一度的高考结束了, 但比高考更重要的是填志愿, 正如在许多时候, 比努力更重要的是选择. 

历年来, 考生们饱受各种奇奇怪怪的志愿填报网站的困扰, 里面的数据参差不齐, 提供的建议也摸棱两可, 而且许多网站趁机敲诈勒索, 让许多考生们浪费了不少金钱的同时, 还花费了很多心力. 

我本人, 也一度饱受这种无良机构的困扰. 

如今, 我手里有了一点点并不成熟的技术, 所以打算写一个这样的系统给大家, 也许它并不好用, 但**它除了接受完全自愿的打赏外, 完全免费**. 

这个系统开发时间太短了, 只有不到一个星期, 最开始的时候全部工作都只能我一个人来做, 所以这个系统难免会有很多奇奇怪怪的Bug.  我也在尽全力解决它们. 如果您觉得好用, 请推荐给周围需要的人, 如果您愿意, 也可以通过扫描打赏链接请我喝杯咖啡. 如果您遇到了什么问题, 也可以通过下方的联系方式联系到我. 

这个系统完全由我自己用课余时间手动开发, 每一行代码都是我一个一个码出来的. 同时, 租用服务器和数据库, 以及给大家注册和重置密码的邮件发送服务, 都是一笔不小的费用. 所以, 我真的非常渴望大家的支持. 

希望这个自己业余时间折腾的这个系统能帮得上你. 如果您希望加入我们, 为这个系统出一份力, 可以参考**贡献指南**, 我们真的很需要大家的帮助. 



### 请求帮助!!

​	尽管我们已经使尽浑身解数, 但我们依然遇到了难以突破的瓶颈, 这严重影响了我们的工作进度和上线进度. 为此, 我们非常需要来自开源世界的帮助. 

​	我们目前已经收集了来自广西和福建两省往年的高考录取数据, 并转换成了图片格式. 我们还利用来自腾讯云的表格识别API, 对所有图片完成了OCR. 但由于OCR的准确率有限, 我们需要对OCR的结果进行整理、校对和结构化处理。在过去的数天时间里，我和我的一位同仁鏖战了数个通宵，依然没能完全解决脏数据带来的各种结构化数据过程中的障碍。我们希望来自开源世界有能力的朋友能够帮助我们。

​	如果您有任何方式，能够将我们目前的扫描的jpg图片数据，或经过OCR的json数据（注意，json数据中，有大量缺漏错的数据）进行清洗和结构化，使之能够结构化为

【院校名称，院校代码， 录取批次，年份，专业，计划数，录取数，录取分数，对应分数的录取人数】

的格式，或

【院校名称，院校代码，录取批次，年份，专业，最低分，最高分，平均分】

的格式，并保存为.csv文件请向我们的项目提交pull request. 

我们非常感谢来自开源世界的你的帮助，我们将会视贡献质量对贡献进行标记，并分享这个开源项目获得的捐助。

json数据的获取地址为：https://data-vankyle-1257862518.cos.ap-shanghai.myqcloud.com/image/gaokao/guangxi-2020/zhinan/json/guangxi-2020_【编号】.json

jpg格式的图片数据获取地址为https://data-vankyle-1257862518.cos.ap-shanghai.myqcloud.com/image/gaokao/guangxi-2020/zhinan/jpg/guangxi-2020_【编号】.jpg

其中，有效编号为25-1067。

请注意，图片数据的版权归原作者所有，未经原作者允许，不允许用于其他用途。对滥用图片数据的行为，原作者保留其追究侵权责任的权力。

### 贡献指南

如果您希望帮助我们继续完善丰富这个系统, 可以考虑通过以下几种方式参与贡献: 

1. 您可以利用您的技术, 为这个项目贡献代码, 修复Bug的同时, 丰富这个系统的功能, 完善各项服务, 优化性能和用户体验等. 方法是直接向这个仓库提交 Pull Request. 
2. 您也可以提供各省市的录取数据, 以丰富这个系统的数据库, 方便更多的考生. 我们会在另外一篇文档中整理贡献数据格式的具体要求, 您也可以通过Issue提供更好的想法. 
3. 您可以直接通过网页中的打赏链接或者Github打赏通道为这个项目打赏, 支持我们支付网站运营的服务器、数据库、邮件服务等费用。网站的费用支出通常会更新在网站的相应页面中，也欢迎大家的监督。
4. 您也可以将您在使用或参与过程中发现的问题整理成Issue，在本仓库的Issue部分提出，方便我们更好地寻找问题的根源，并尽早解决问题。
5. 您可以告诉您身边的人，让更多的人关注到我们甚至加入我们。

您的支持是我们不断前进的动力。





