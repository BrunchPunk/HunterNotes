charmsFile = open('charm.txt', 'r', encoding='utf-8')
charmsArmorResultsFile = open('charmArmor.csv', 'w', encoding='utf-8')
charmsForgeResultsFile = open('charmForge.csv', 'w', encoding='utf-8')

charmName = ""
charmEquipSlot = '"Charm"'
charmSkill1Name = '""'
charmSkill1Points = ""
charmSkill2Name = '""'
charmSkill2Points = ""
charmSkill3Name = '""'
charmSkill3Points = ""
charmSkill4Name = '""'
charmSkill4Points = ""
charmIngredient1 = '""'
charmIngredient1Count = ""
charmIngredient2 = '""'
charmIngredient2Count = ""
charmIngredient3 = '""'
charmIngredient3Count = ""
charmIngredient4 = '""'
charmIngredient4Count = ""
decorationSlot1 = '""'
decorationSlot2 = '""'
decorationSlot3 = '""'

skillCount = 1
ingredientCount = 1

'''
parse modes
1 = charm name line
2 = skill names/value lines
3 = materials
4 = write line and reset counters
'''
parseMode = 1

for line in charmsFile:
    if(parseMode == 1): #Get charm Name
        if(not (line == "\n")):
            charmName = '"' + line.rstrip() + '"'
        else:
            parseMode = 2
    elif(parseMode == 2): #Get charms skills/values
        if(not (line == "\n")):
            if(skillCount == 1):
                charmSkill1Name = '"' + line[:-3].rstrip() + '"'
                charmSkill1Points = line[-2:].rstrip()
                skillCount = skillCount + 1
            elif(skillCount == 2):
                charmSkill2Name = '"' + line[:-3].rstrip() + '"'
                charmSkill2Points = line[-2:].rstrip()
                skillCount = skillCount + 1
            elif(skillCount == 3):
                charmSkill3Name = '"' + line[:-3].rstrip() + '"'
                charmSkill3Points = line[-2:].rstrip()
                skillCount = skillCount + 1
            elif(skillCount == 4):
                charmSkill4Name = '"' + line[:-3].rstrip() + '"'
                charmSkill4Points = line[-2:].rstrip()
                skillCount = skillCount + 1
        else:
            parseMode = 3
    elif(parseMode == 3): #Get charms ingredients
        if(not (line == "\n")):
            if(ingredientCount == 1):
                charmIngredient1 = '"' + line[:-3].rstrip() + '"'
                charmIngredient1Count = line[-2:].rstrip()
                ingredientCount = ingredientCount + 1
            elif(ingredientCount == 2):
                charmIngredient2 = '"' + line[:-3].rstrip() + '"'
                charmIngredient2Count = line[-2:].rstrip()
                ingredientCount = ingredientCount + 1
            elif(ingredientCount == 3):
                charmIngredient3 = '"' + line[:-3].rstrip() + '"'
                charmIngredient3Count = line[-2:].rstrip()
                ingredientCount = ingredientCount + 1
            elif(ingredientCount == 4):
                charmIngredient4 = '"' + line[:-3].rstrip() + '"'
                charmIngredient4Count = line[-2:].rstrip()
                ingredientCount = ingredientCount + 1
        else:
            parseMode = 4
    elif(parseMode == 4): #Write line and Reset
        charmsArmorResultsFile.write(charmName + ',' +
                              charmEquipSlot + ',' +
                              charmSkill1Name + ',' +
                              charmSkill1Points + ',' +
                              charmSkill2Name + ',' +
                              charmSkill2Points + ',' +
                              charmSkill3Name + ',' +
                              charmSkill3Points + ',' +
                              charmSkill4Name + ',' +
                              charmSkill4Points + ',' +
                              decorationSlot1 + ',' +
                              decorationSlot2 + ',' +
                              decorationSlot3 + "\n")

        charmsForgeResultsFile.write(charmName + ',' +
                                     charmIngredient1 + ',' +
                                     charmIngredient1Count + ',' +
                                     charmIngredient2 + ',' +
                                     charmIngredient2Count + ',' +
                                     charmIngredient3 + ',' +
                                     charmIngredient3Count + ',' +
                                     charmIngredient4 + ',' +
                                     charmIngredient4Count + "\n")
                                     

        charmName = ""
        charmEquipSlot = '"Charm"'
        charmSkill1Name = '""'
        charmSkill1Points = ""
        charmSkill2Name = '""'
        charmSkill2Points = ""
        charmSkill3Name = '""'
        charmSkill3Points = ""
        charmSkill4Name = '""'
        charmSkill4Points = ""
        charmIngredient1 = '""'
        charmIngredient1Count = ""
        charmIngredient2 = '""'
        charmIngredient2Count = ""
        charmIngredient3 = '""'
        charmIngredient3Count = ""
        charmIngredient4 = '""'
        charmIngredient4Count = ""
        decorationSlot1 = '""'
        decorationSlot2 = '""'
        decorationSlot3 = '""'

        skillCount = 1
        ingredientCount = 1
        
        parseMode = 1

charmsFile.close()
charmsArmorResultsFile.close()
charmsForgeResultsFile.close()
