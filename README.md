# ConsoleMiniGame
  This is the Console Mini Game I wrote as part of freeCodeCamp's Foundational C# With Microsoft course, improved with my own touches.

  Ideas for improvement:

  - Victory condition: Instead of reaching X "bad" foods on the console, use points. +1 for each "good" food consumed, -1 for each "bad" one.
  - FreezePlayer could flicker the player model. So after printing the (X_X) face, it goes ahead and prints the ('-') face as is, but then sleep(100 or so), move cursor to its beginning, replace with whitespace, sleep(100 or so) move cursor to its beginning and print the ('-') face anew. So it looks like it flickers once.
  - Make the bottom-most line not accessible to either the player model or the food spawning method and use it only to keep the current score displayed.
  - Character speedup when character is (^-^), having eaten the "$$$$$" food: as is, the increased movement will skip a line or two and the player will need to move the player model all the way to the top to have access to the even lines or all the way to the bottom to have access to the odd ones. That could be improved by checking the player model after the ChangePlayer method is called. If it's (^-^), instead of just spawning food with the current code, use maybe a while loop to make sure that the Y coordinate of the food item will be even if the player's is also even, or odd if the player's is also odd.
