﻿

### index
	查成绩: 
		1. 给自己查->读你自己的数据(input model)
		2. 给别人查->填别人的数据
		* 你多少分
		* 你现在多少名
		* 你能容忍的波动范围
			* 排名
			* 线差
		* 查大学
		* 查专业
		* 查省市

	计算能上的范围:
		1. 分数对应的排名
			* 最大值
			* 最小值
		2. 线差
			* 最大值
			* 最小值
	
	查数据库, 能找到的所有符合以下要求的专业, include学校
		查询条件: 
		* 符合考生查询的门类

		* 分数小于分数最大值
		* 分数大于分数最小值
							====>出来一个List

		* 线差小于线差最大值
		* 线差小于线差最小值
							====>出来一个List

		这两个List会包含所有能上的专业, 数据进Report/Index
		列出来应该是这样: 
		| 学校 | 专业名称 | 最高分 | 最低分 | 线差 | 排名 | 操作 |

	从Report/Index提供几个进入具体报告的入口
		1. 学校 (Report/School) 
			把List中所有的学校列出来, 显示学校信息, 外加一条专业名称
		2. 具体的专业 (Report/Major)
			把List中所有的专业列出来, 显示专业信息, 外加一条学校名称
		3. 省份 (Report/Provice)(暂未开放)
		4. 城市 (Report/City) (暂未开放)

 