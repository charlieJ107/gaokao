import json
import csv

#全局变量
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
    f = open(".\\ocr\\guangxi-2019\\zhinan\\json\\guangxi-2020_"+str(i)+".json",encoding='utf-8')
    sets = json.load(f)["TextDetections"]
    sheet = [[0 for i in range(20)] for j in range(20)]
    for set in sets:
        sheet[int(set["RowTl"])][int(set["ColTl"])] = set["Text"]
    return sheet

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
    #判断分数是否读完的Tag
    global continuetag

    for i in range(20):
        if "院校代码" in str(sheet[i][0]):
            temp = str(sheet[i][0]).split(":")
            lyear = year = temp[1].replace(" ","").replace("院校代码","")
            lnum = num = temp[2].replace(" ","").replace("院校名称","")
            lname = name = temp[3].replace(" ","").replace("录取批次","")
            lbatch = batch = temp[4].replace(" ","")
        
            mojarSource = str(sheet[i+2][0]).split("\n")
            mojar = []

            Nexttag = False
            for s in range(len(mojarSource)):
                if Nexttag:
                    Nexttag = False
                    continue
                else:
                    if mojarSource[s].count("(") != mojarSource[s].count(")"):
                        mojar.append(str(mojarSource[s])+str(mojarSource[s+1]))
                        Nexttag = True
                    else:
                        mojar.append(str(mojarSource[s]))
        
            #判断分数是不是断节的
            scoreTemp = str(sheet[i+2][4])
            if scoreTemp[len(scoreTemp)-1] != ".":
                #标记分页时需要先处理分数
                continuetag = True

            scheSource = str(sheet[i+2][1]).split("\n")
            enrollSource = str(sheet[i+2][3]).split("\n")
            scoreSource = str(sheet[i+2][4]).split(".")
        
            result = []

            k=0
            for s in range(len(mojar)):
                if enrollSource[s] == "0":
                    result.append([year,name,num,batch,mojar[s],scheSource[s],enrollSource[s]])
                else:
                    score = str(scoreSource[k]).replace("\n","").split(";")
                    k = k+1
                    for sco in score:
                        if str(sco) != "":

                            res = str(sco).split(":")
                            result.append([year,name,num,batch,mojar[s],scheSource[s],enrollSource[s],res[0],res[1]])



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
    scoreSource = str(sheet[0][4]).split(".")
        
    result = []
    k = 0
    if continuetag:
        k = 1
        score = str(scoreSource[0]).replace("\n","").split(";")
        for sco in score:
            res = str(sco).split(":")
            result.append([lyear,lname,lnum,lbatch,lmojar,lsche,lenroll,res[0],res[1]])
        continuetag = False
    
    mojar = []
    Nexttag = False
    for s in range(len(mojarSource)):
        if Nexttag:
            Nexttag = False
            continue
        else:
            if mojarSource[s].count("(") != mojarSource[s].count(")"):
                mojar.append(str(mojarSource[s])+str(mojarSource[s+1]))
                Nexttag = True
            else:
                mojar.append(str(mojarSource[s]))

    for s in range(len(mojar)):
        if enrollSource[s] == "0":
            result.append([lyear,lname,lnum,lbatch,mojar[s],scheSource[s],enrollSource[s]])
        else:
            score = str(scoreSource[k]).replace("\n","").split(";")
            k = k+1
            for sco in score:
                if str(sco) != "":
                    res = str(sco).split(":")
                    result.append([lyear,lname,lnum,lbatch,mojar[s],scheSource[s],enrollSource[s],res[0],res[1]])

    allresult = allresult+result

for i in range(22,1083):
    try:
        sheet = loadjson(i)
    except IndexError as error:
        with open("./err.log",'a', encoding='utf-8') as f:
             f.write("IndexError: In newschool, json: "+str(i)+"\n")
        continue
    except FileNotFoundError:
        continue
    if str(sheet[0][4]) != "0":
        try:
            continuepage(sheet)
        except IndexError as error:
            with open("./err.log",'a', encoding='utf-8') as f:
                f.write("IndexError: In newschool, json: "+str(i)+"\n")
            continue
    try:
        newschool(sheet)
    except IndexError as error:
        with open("./err.log",'a', encoding='utf-8') as f:
            f.write("IndexError: In newschool, json: "+str(i)+"\n")
        continue

print(allresult)
with open("./result.csv", 'w', encoding='utf-8') as f:
    writer = csv.writer(f, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
    for i in allresult:
        writer.writerow(i)