import re


class NotMatchException(Exception):
    def __init__(self, unmatchString):
        self.string=unmatchString



class EnrollSourceLackError(Exception):
    def __init__(self, description, data):
        self.string=description
        self.data=data

class EnrollDataLackError(Exception):
    def __init__(self, description, data):
        self.string=description
        self.data=data

class ScheduleDataLackError(Exception):
    def __init__(self, description, data):
        self.string=description
        self.data=data
class MajorDataLackError(Exception):
    def __init__(self, string, data):
       self.string=string
       self.data=data

class UnknownError(Exception):
    def __init__(self, string, data):
        self.string=string
        self.data=data

def getBeautifulYuanxiao(tirtyString):
    '''
    院校信息那一行有问题, 用正则表达式来处理
    参数 那个脏的院校行
    '''

    bestPatten=[r".*年份.*(\d{4}).*院校代码.*(\d{5}).*院校名称:?([\u0020\u4e00-\u9fa5]*).*录取批次:?(.*)", 
                r".*年份.*(\d{4}).*院校代码.*(\d[^0-9]*\d[^0-9]*\d[^0-9]\d[^0-9]\d).*院校名称:?([\u0020\u4e00-\u9fa5]*).*录取批次:?(.*)"]
    for pattenIndex in range(len(bestPatten)):
        matchObj = re.match(bestPatten[pattenIndex],tirtyString)
        if(matchObj==None):
            continue
        else:
            return matchObj.groups()
    raise NotMatchException(tirtyString)

def getPrettyScoreSource(dirtyScoreSource):
    beautifulString = ""
    wait2Write = ""
    backupString = ""

    standardPatten = re.compile(r"[^:;0-9]*((\d{3})[^0-9]*(\d*)[^0-9]*).*")
    isMajorEndPatten=re.compile(r".*[.]$")
    doubleGroupPatten=re.compile(r"[^\u002E:0-9]*(\d{3})[^:0-9]*:[^:0-9]*(\d+?)[^;0-9]*(\d{3})[^:0-9]*:[^:0-9]*(\d+)[^0-9]*")

    for dirtyLine in dirtyScoreSource.splitlines():
        dirtyLine = dirtyLine.replace('I', '1').replace('i', '1')
        isMajorEnd = isMajorEndPatten.match(dirtyLine)
        if isMajorEnd==None:
            isMajorEndFlag=False
        else:
            isMajorEndFlag=True
        while len(dirtyLine)>0:
            standardMatchObj=standardPatten.match(dirtyLine)
            if standardMatchObj == None:
                #正常的东西失配, 开始进行异常处理
                # 先把上次匹配的东西加回来
                dirtyLine = backupString + dirtyLine
                # 这个正则可以找出中间缺少分号的两组
                doubleGroupObj = doubleGroupPatten.match(dirtyLine)
                if doubleGroupObj == None:
                    raise NotMatchException(dirtyLine)
                else:
                    wait2Write = doubleGroupObj.groups()[0] + ':' + doubleGroupObj.groups()[1] + ';' + doubleGroupObj.groups()[2] + ':' + doubleGroupObj.groups()[3] + ';'
                    where2Cut=dirtyLine.find(doubleGroupObj.group(0))+len(doubleGroupObj.group(0))
                    dirtyLine = dirtyLine[where2Cut:]
            else:
                beautifulString += wait2Write
                where2Cut = dirtyLine.find(standardMatchObj.group(1)) + len(standardMatchObj.group(1))
                backupString = dirtyLine[0:where2Cut]

                wait2Write = standardMatchObj.group(2) + ':' + standardMatchObj.group(3) + ';'
                
                dirtyLine = dirtyLine[where2Cut:]

        if isMajorEndFlag:
            if len(beautifulString)==0:
                beautifulString += wait2Write + '.\n'
                wait2Write=""
            else:
                wait2Write += '.\n'
    return beautifulString
            