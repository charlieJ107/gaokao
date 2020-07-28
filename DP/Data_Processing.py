import json
import re
import csv
from solveException import *

#全局变量
#最终结果集合
allresult = []
lyear = ""
lnum = ""
lname = ""
lbatch = ""
lmojar = ""
lsche = ""
lenroll = ""
#判断分数是否读完的Tag
continuetag = False

def loadjson(i):
    f = open("./ocr/guangxi-2019/zhinan/json/guangxi-2020_"+str(i)+".json",encoding='utf-8')
    print("guangxi-2020_"+str(i)+".json")
    sets = json.load(f)["TextDetections"]
    sheet = [[0 for i in range(40)] for j in range(40)]
    #按照json内的行列信息 用二维数组构建一个虚拟表格
    for set in sets:
        sheet[int(set["RowTl"])][int(set["ColTl"])] = set["Text"]
    return sheet

def lineFeedFix(mojarSource):
    mojar = []

    Nexttag = False
    #把换行专业合并
    for s in range(len(mojarSource)):
        #被合并的那一行 跳过处理
        if Nexttag:
            Nexttag = False
            continue
        else:
            if mojarSource[s].count("(") != mojarSource[s].count(")"):
                mojar.append(str(mojarSource[s])+str(mojarSource[s+1]))
                Nexttag = True
            else:
                mojar.append(str(mojarSource[s]))
    return mojar

def getFlag(sheet,i):
    Flag = i+1;
    if "科目组及专业名称" in str(sheet[i+1][0]):
        Flag = i+2
    return Flag

def newschool(sheet):
    #全局变量
    global allresult
    global lyear
    global lnum
    global lname
    global lbatch 
    global lmojar 
    global lsche 
    global lenroll 
    #判断分数是否读完的Tag，如果这个tag为true 说明下一页页头 第一栏分数部分 是属于上一页上次读到的那个专业的
    global continuetag

    #这是在处理院校，在每页内一行一行扫描，发现有“院校代码”就可以判断是学校信息行了
    for i in range(20):
        if "院校代码" in str(sheet[i][0]):
            temp=getBeautifulYuanxiao(sheet[i][0])
            lyear = year = temp[0].replace(" ","").replace("院校代码","")
            lnum = num = temp[1].replace(" ","").replace("院校名称","")
            lname = name = temp[2].replace(" ","").replace("录取批次","")
            lbatch = batch = temp[3].replace(" ","")
            
            #计算是否有标题行
            Flag = getFlag(sheet,i)

            mojarSource = str(sheet[Flag][0]).split("\n")
            #此处处理换行专业
            mojar = lineFeedFix(mojarSource)
        
            #判断分数是不是断节的
            scoreTemp = str(sheet[Flag][4])
            try: 
                if scoreTemp[len(scoreTemp)-1] != ".":
                    #标记分页时需要先处理分数
                    continuetag = True
            except IndexError:
                raise UnknownError("这里很可能把一个没有录取数据的乱码当作分数了, 需要处理一下, 怀疑是由于改了标题行相关代码后导致的", sheet[Flag]) 


            scheSource = str(sheet[Flag][1]).split("\n")
            enrollSource = str(sheet[Flag][3]).split("\n")
            scoreSource = getPrettyScoreSource(str(sheet[Flag][4])).split('.')
        
            result = []

            k=0
            #读取每个专业下的分数进行处理
            if len(mojar)==len(enrollSource):
                for s in range(len(mojar)):
                    #如果录取数为0 不对分数分布进行处理
                    if enrollSource[s] == "0":
                        result.append([year,name,num,batch,mojar[s],scheSource[s],enrollSource[s]])
                    else:
                        #把分数分布切出来
                        try:
                            score = str(scoreSource[k]).replace("\n","").split(";")
                        except IndexError:
                            raise EnrollSourceLackError("这部分录取数据很可能有个专业结束的地方漏了一个点", str(sheet[Flag][4]))
                        #这是个内部计数器 因为如果出现了录取为0的情况 那一行是空行 也就是说 分数的数组成员会比专业的少 为了防止溢出 单独用计数器匹配
                        k = k+1
                        #取出分数 然后按:分割
                        for sco in score:
                            if str(sco) != "":
                                res = str(sco).split(":")
                                try:
                                    result.append([year,name,num,batch,mojar[s],scheSource[s],enrollSource[s],res[0],res[1]])
                                except IndexError:
                                    if len(mojar)<=s:
                                        raise MajorDataLackError("这里的专业数据可能缺失, 大概在"+mojar[s-1]+'附近',(s, mojar))
                                    elif len(scheSource)<=s:
                                        raise ScheduleDataLackError("这里的\'计划数\'可能缺失, 在"+scheSource[s-1]+'附近', (s, scheSource))
                                    elif len(res)<=2:
                                        raise EnrollSourceLackError("这里的录取数据应该有漏的, 在"+major[s]+"附近",sco)
                                    else:
                                        raise EnrollDataLackError("这里应该是有错的, 大概率是录取数据有漏的, 在"+str(major[s])+"附近", score)
            else:
                # 想来想去, 这个不是数据不全, 而是数据漏了, 没什么好的办法, 只能手动改数据了...
                raise EnrollSourceLackError("这里应该是有录取数据漏了整行, 导致专业数和录取数不一致, 在"+lname+"附近", enrollSource)


                lsche = scheSource[s]
                lenroll = enrollSource[s]

            allresult = allresult+result

def continuepage(sheet):
    #全局变量
    global allresult
    global lyear
    global lnum
    global lname
    global lbatch 
    global lmojar 
    global lsche 
    global lenroll 
    #判断分数是否读完的Tag
    global continuetag

    mojarSource = str(sheet[0][0]).split("\n")
    scheSource = str(sheet[0][1]).split("\n")
    enrollSource = str(sheet[0][3]).split("\n")
    scoreSource = getPrettyScoreSource(str(sheet[0][4])).split(".")
        
    result = []
    k = 0
    if continuetag:
        k = 1
        score = str(scoreSource[0]).replace("\n","").split(";")
        for sco in score:
            res = str(sco).split(":")
            try:
                result.append([lyear,lname,lnum,lbatch,lmojar,lsche,lenroll,res[0],res[1]])
            except IndexError:
                raise EnrollSourceLackError("这个地方的录取数据可能漏了点"+str(lname), sco)

        continuetag = False
    
    mojar = lineFeedFix(mojarSource)

    if len(mojar)==len(enrollSource):
        for s in range(len(mojar)):
            if enrollSource[s] == "0":
                result.append([lyear,lname,lnum,lbatch,mojar[s],scheSource[s],enrollSource[s]])
            else:
                try:
                    score = str(scoreSource[k]).replace("\n","").split(";")
                except IndexError:
                    raise EnrollSourceLackError("这部分录取数据很可能有个专业结束的地方漏了一个点", str(enrollSource))
                k = k+1
                for sco in score:
                    if str(sco) != "":
                        res = str(sco).split(":")
                        try:
                            result.append([lyear,lname,lnum,lbatch,mojar[s],scheSource[s],enrollSource[s],res[0],res[1]])
                        except IndexError:
                            if len(mojar)<=s:
                                raise MajorDataLackError("这里的专业数据可能缺失, 大概在"+mojar[s-1]+'附近',(s, mojar))
                            elif len(scheSource)<=s:
                                raise ScheduleDataLackError("这里的\'计划数\'可能缺失, 在"+scheSource[s-1]+'附近', (s, scheSource))
                            elif len(res)<=2:
                                raise EnrollSourceLackError("这里的录取数据应该有漏的, 在"+major[s]+"附近",sco)
                            else:
                                raise EnrollDataLackError("这里应该是有错的, 大概率是录取数据有漏的, 在"+str(major[s])+"附近", score)
    else:
        # 想来想去, 这个不是数据不全, 而是数据漏了, 没什么好的办法, 只能手动改数据了...
        raise EnrollSourceLackError("这里应该是有录取数据漏了整行, 导致专业数和录取数不一致, 在"+lname+"附近", enrollSource)

    allresult = allresult+result

for i in range(25,1067):
    try:
        try:    
            sheet = loadjson(i)
        except FileNotFoundError:
            continue
        if str(sheet[0][4]) != "0":
            continuepage(sheet)
        newschool(sheet)
    except NotMatchException as NotMatch:
        with open("NotMatchLog.txt", 'a', encoding='utf-8') as log:
            log.write("NotMatchError: in "+str(i)+" json, with: "+str(NotMatch.string)+'\n')
    except EnrollSourceLackError as EnrollLack:
        with open("EnrollSourceLack.txt", 'a', encoding='utf-8') as log:
            log.write("""
            ============\n
            EnrollLack: in """+str(i)+" json, \nwith message: " + str(EnrollLack.string) + "\n of data: \n"+ str(EnrollLack.data)+"""\n
            ============""")
    except MajorDataLackError as error:
        with open("MajorLack.txt", 'a', encoding="utf-8") as log:
            log.write("""
            ============\n
            MajorLack: in """+str(i)+" json, \nwith message: " + str(error.string) + "\n of data: \n"+ str(error.data)+"""\n
            ============""")
    except ScheduleDataLackError as error:
        with open("Schedule.txt", 'a', encoding="utf-8") as log:
            log.write("""
            ============\n
            ScheduleLack: in """+str(i)+" json, \nwith message: " + str(error.string) + "\n of data: \n"+ str(error.data)+"""\n
            ============""")
    except EnrollDataLackError as error:
        with open("EnrollDataLack.txt", 'a', encoding="utf-8") as log:
            log.write("""
            ============\n
            EnrollDataLack: in """+str(i)+" json, \nwith message: " + str(error.string) + "\n of data: \n"+ str(error.data)+"""\n
            ============""")
    except UnknownError as error:
        with open("UnkownError.txt", 'a', encoding="utf-8") as log:
            log.write("""
            ============\n
            UnkownError: in """+str(i)+" json, \nwith message: " + str(error.string) + "\n of data: \n"+ str(error.data)+"""\n
            ============""")


    #except IndexError as OutOfIndex:
    #    with open("IndexErrorLog.txt", 'a', encoding='utf-8') as log:
    #        log.write("IndexError: in"+str(i)+"json, with: "+str(OutOfIndex)+'\n')
#with open("./result.csv", 'w', encoding='utf-8') as csvFile:
#    writer = csv.writer(csvFile, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
#    for i in allresult:
#        writer.writerow(i)