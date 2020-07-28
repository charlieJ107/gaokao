### 这是一个调试的时候用来测试有问题的代码片段的文件, 不用理我####

import re


class NotMatchException(Exception):
    def __init__(self, unmatchString):
        self.string=unmatchString
def getPrettyScoreSource(dirtyScoreSource):
    beautifulString = ""
    wait2Write = ""
    backupString = ""

    standardPatten = r"[^:;0-9]*((\d{3})[^0-9]*(\d*)[^0-9]*).*"
    for dirtyLine in dirtyScoreSource.splitlines():
        dirtyLine = dirtyLine.replace('I', '1').replace('i', '1')
        isMajorEndFlag = False
        isMajorEndPatten=r".*[.]$"
        isMajorEnd = re.match(isMajorEndPatten, dirtyLine)
        if isMajorEnd==None:
            isMajorEndFlag=False
        else:
            isMajorEndFlag=True
        while len(dirtyLine)>0:
            standardMatchObj=re.match(standardPatten, dirtyLine)
            if standardMatchObj == None:
                #正常的东西失配, 开始进行异常处理
                # 先把上次匹配的东西加回来
                dirtyLine = backupString + dirtyLine
                # 这个正则可以找出中间缺少分号的两组
                doubleGroupPatten=r"[^\u002E:0-9]*(\d{3})[^:0-9]*:[^:0-9]*(\d+?)[^;0-9]*(\d{3})[^:0-9]*:[^:0-9]*(\d+)[^0-9]*"
                doubleGroupObj = re.match(doubleGroupPatten, dirtyLine.replace('I', '1').replace('i','1'))
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
            beautifulString += '.\n'
    return beautifulString

dirtyScoreSource = '718: 1;716:1;710:1;708: I ;706:1; 704:1 ;703:1;\n701:2;700:2;699:1.\n701:1;700:2;699:1.\n710:1;703:1.\n704:1.\n706:1.\n718:1;716:1;701:1.\n708:1.\n689 :1;685:1;678:1;670:1;666:1;664:2;663:1;\n662:1;660:1;659:1;658:2;657:2.\n678:1.\n685:1.\n689:1;666:1.\n663:1.\n664:1.\n659:1;658:1.\n657:1.\n660:1.\n662:1;657:1.\n658:1.\n664:1.\n670:1.'

pretty = getPrettyScoreSource(dirtyScoreSource)
print(pretty)