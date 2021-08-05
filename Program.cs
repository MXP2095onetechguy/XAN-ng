using System;
using System.Threading;
using System.IO;
using RNGGen;
using Kurukuru;
using Serilog;
using WidgetToolkit;
using System.Diagnostics;


/* JSUlJSUlJSUlJUBAJiYmJiYmJSMvLy8vLyoqKiwqLy8vLy8vLy8vLy8vLy8vLy8vLy8vLygvKiwsKioqKioqKioqLy8vLy8vLygoIyUlJiUlIyMjIyMjIyMjIyMjIyMjIyMjIwolJSUlJSUlJSUlJkAmJiYmJiYmJiYmJSgoKCgoIyMoLy8vLy8vLy8vLy8vLy8vLy8vLy8oKCgvKiwqKioqKioqKiovLy8vLy8oKCgjJSUlJSMjIyMjIyMjIyMjIyMjIyMjIyMjCiUlJSUlJSUlJSUlJSUlJSUlJSUlIygoKCgoKCgoKC8vLy8vLy8vLy8vLy8vLy8oIyMjIyUlJSUlKiwqKioqKioqKi8vLy8vLygoKCMlJSMjIyMjIyMjIyMjIyMjIyMjIyMjIyMKJSMoKC8vKioqLyMlJSUlJSUlJSMoLygoKCgoKCgoKC8vLy8vLy8vKCMjIyMjKCMjIyMjJSUjJSUvKioqKioqKioqKi8vLy8vKCgjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIwoqKioqKioqKioqKiglJSUlJSUlIy8vKi8vLy8vKCgoKC8vKCMjIyMoKCgjIyMjKCgjIyMjIyMlJSMqKioqKioqKiovLy8vLy8oKCMjIyMjIyMjIyMjIyUjIyMjIyMjIyMjIyMjCioqKiosLCwsKioqKiMlJSUlJSUlKCoqLCovKCgoKCgjIyMjIyMoKCMjIygjIyMoKCgoIyMjIyUlJS8qKioqKioqKi8vLy8vKCgoIygvIyUmJiUoLy8jJSYlIy8qLy8vKCgoKCgKLCwsLCwsLCwsLCwqKi8oLy8vKi4gICooKCgoKCgoKCwgLiMjIygoKCgoIyMjIyggLigjIyUlJSUlIyoqKioqKioqLy8vLy8oKCMjLyovLy8vLCAgLCoqKi4gLi8jIyMjIyMjIwosLCoqKioqKiwsLCwqKCUlJSUsICAgIC8oKCgoKCgoLCAuKCMjKCMjIyMjIyMjIyMlIyMjIyMjIyMjLyoqKioqKi8vLy8vLygoIyMqLCwsKiosLCoqKiosLiAsIyUlIyMjIyMjCioqKioqKiosLCwsLCoqIyUlLyAuKC8gIC8oKCgoKCgsICAvLy8qKi8oIyMoKCgvKiovKCgoKCgoLy8qLCwqLCwqLy8vLy8vKiovLCwsLCwsLCwuLiwsKiouIColJS8vIyMjIyMKKiosLCoqKiwsLCwsLCwqKCogICgoKC8gIC8oKCgoKCwgICAuLygvKiAgLi8vLy8gICovKCgqICAqKCgqLiAgICwvLy8vICwvKCMsICAuLCwsLCAgLiwsLC4gKiMjLywqKCMjIwoqKioqKioqLCwsLCwsLCwsICAoKCgoKCogLi8oKC8vLiAgKiosLygoKC4gLC8vLyAgKi8vKiAuKCgjIygqKi4gLCovLy8vKCgoKi4uICAsLCwsICAuLCwsLiAqKCgjKCosLyMjCioqKioqKiosLCwsLCwsLCAgLi4uLi4uLiAgLi8qLCwuICAqKiovKCgoLiAsLy8vICAqLy8sICwoKCgjIyoqLiAsKi8vLiAuKigsLCwgICoqKCMgICooKC8uICwvKCgjIy8qKigKLCwsKioqKiwsLCwqKiogICwqKioqKioqKi4gICwsLC4gIC4sKioqLywgLi8vLy8gICovLyogICwoKCMjLy4gIC4qKiwgIC8jKCosLiAgLCoqKiAgLC8vLy4gLCoqKiovKC8qKgosLCwsLCwsLC4sLCwsICAuLCoqKiosLCwqKi4gIC4uICAgLC4gICAgLC8vLy8vLyAgKi8vLy8qLiAgIC4qLC4gLioqLyosICAgIC4sICAuLiwuICAuLi4uICAgLi4uLiwoIygvCiosKiosLCwuLi4sLCwqKiwsLCwsLCwqKioqLCwsLi4uLCwsLCoqLCwqKCgvLy8vLy8vLy8vLygoKCgoIyMqICAqKiovLygjJS8qKioqLC4uKi8vLy8vLyoqKioqKiwuLCgjIyUKLCwsLCwsLC4uIC4uLCwqLCwsLCwsKigjLyovLyosLi4sLCwsLCwsLCovKCgvLy8vLy8vLy8uICAuLi4gICAuKioqLy8oKCMlLyoqKiosLiwqLy8vLy8vLy8qKioqLC4sIyYlJQoqKioqKioqKiwsLCwsLCwsLCwsLCwqKCgvKi8vKiwuLiwsLCwsLCwsLCwvKC8vLy8vLy8vLy8vLy8oKCgjKCoqKiovKCgoIyUvKioqKiwuLCovLy8vLy8vLyoqKiosLiwvIyMjCiwsLCwsLCwsLCwsLCwsLCwsLCwsLCovLy8vLy8vKiwuLCwsLCwsLCwsLCooKC8vLy8vLy8vLy8vLy8oKCgjKioqKi8vKCgjIyoqKioqLC4sKi8vLy8vLyoqKioqKiwuLC8jIygKLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCovLy8vLy8qLC4sLCwsLCwsLCwsLC8oLy8vLy8vLy8vLy8vLygoKCMvKioqKi8oIyMjKioqKiosLiwqKioqKioqKioqKioqLC4sLyMjKAosLCwsLCwsLCwsLCwsLCwsLCwsLCwqLy8oKCgvLy8qLiwsLCwsLCwsLCwqLygoLy8vKCgoKCgoKC8vKCgoIygqKiovLygjIyMvKioqKiwuLCwsLCwsLCwsLCwsLCwsLiwqKCMjCiwsLCwsLCwsLCwsLCwsLCwsLCwsLCovKCgoKCgoKC8uLi4uLi4uLi4uLi4oKC8vLygoKCgoKCgoKCgoKCMjLyoqLy8vKCgjIywuLi4uLiwsKi8oKCgjIygoIyMjIygqLCojIyUKLCwsLCwsLCwsLCwsLCwsLCwsLCwsKigoKCgoKCgoLywuLC8oKCgoIyMjKCgvLy8vKCgoKCgoKCgoKCgjIyoqKiovLy8vKCgjJiYmJSMvLCwqKCgoIyMjIyUlJiUlJSgqLyYlJQosLCwsLCwsLCwsLCwsLCwsLCwsLCwqKCgoKCgoKCgoKi4sLygoKCMjIygvLy8vLygoKCgoKCgoKCgoIygqKioqLy8vLy8oKCMjIyMjIygsLC8lJSMlJkBAJSMlJkAmIyovJSUlCiwsLCwsLCwsLCwsLCwsLCwsLCwsKi8oIygoKCgoKCgvLiwvKCgoIyMoLy8vLy8oKCgoKCgoKCgoIyMoKioqKioqLy8vLygoIyZAJiYjIywsKCYlJSZAQEAmJSZAQEAlKiolJSMKLCwsKiovKCgjIygsLCgjIygsLiooIyMjIyMjIyMjIyguLC8oKCgoKC8vLy8vLy8oKCgoKCgoIyMlJS8qKioqKi8vLy8oKCgjJkAmJSMlKiwoJSMlJkBAJiUjJSYmJiUqLCMlIwolJSUlJSUlIyMjIyoqKCMjIy8uLCgjIyMjIyMjIyMjKCwsKigoKC8vLy8vLy8vLygoKCgoIyMlJSUjKCgjIygoIyMjIyMlJSUmJiYlJkAvLCglIyZAQEBAJiUmQEBAJigvIyUjCiUlJSMlIyMjIyMoLyooIygoLywsKCgoKCgoKCgoKCgoKi4qLygvLy8vLy8vKCgoKCgoKCMjJSUlJSMoKCgoKCMjIyMjIyMlJSUmJiYmJiMqKCUlJSYmJiUlJSUlJSUlIyMlJSUKIyMjIyMjIyMjIygvKiglIyMoLCooKCgoKCgoKCgoKCgqLiovLy8vLy8vKCgoKCgoKCgoIyUjIyUjIyMjKCMjIygjIyMjJSUlJiYmJiYlKiooJSUmJiYmJiYlJkBAQEBAJSUlJQojIyMjIyMjIyMjIygvIyUlJSUoKCMjIyMjIyMjIyMjIygoKCgvLygvKCgoKCgoKCgoKCMlIygoJSYlJSUjIyMlJSUlJSUlJSYmJiYmQEBAJiYmJkBAQEAmJiUlJSUlJSMjIyUlCiUlJSUlJSUlJSUlIygjJiYlJSMoIyUlIyMjIyMjIyMjIygoLy8oKCgoKCgoKCgoKCMjJSUjKCgjIyMoKCgjIyMjJSUlJSYlJiYmJiYmJiUjJSUlJSUlJSUlJSUlJSUjIyMjJSUKKCgoIyMjIyMjIyMoKCgjIyMjIyMlJiYmJiYlJSUlJSUjKCgvKCgoKCgoKCgoKCgjIyUmJiYlIyUmJiMoKCgoKCgoKCMjJSUlIyMlJSUlIyMjJSUlJSUlJSUlJSUlJSMjIyMlJQooKCgoKCgoKCMoKCgoKCgoIyMoIyUmJiYmJiYlJSUlJSMoLygoKCgoKCgoKCgoKCMlJiYlJSMlJSUjIygvLy8oKCgoKCMlJSUjIyUlJSUjIyMjJSUlJSUlJSUlJSYmQEAmJSUlCiMjIyMjIyMjIyMjIyMjIyMjIyMjIyUmJiYmJiYmJiUlKCgoKCgoKCgoKCgoKCMjJSUlIyUlJSUlJSUjKCgvKCgoKCgjIyUlJSUlJSUlJSUjIyUmQEBAQCYlJSMlJSUlJSUlJSYKIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMmQCYmJiYmJiMoKCgoKCgoKCgoKCgjIyUlJSUlJSUlJSUlJSMoKC8oKCgoKCMjJSYmQEBAJiYmJSMjJSUmJiYmJSUlJkBAQEBAQCYlJgojIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyUmJiYmJiYmKCgoKCgoKCgoKCgoIyMlJSMlJSUlJSYmJiYlIygoLygoKCgoIyUmJiYmJiYmJiUlJkBAQEBAQEAmJSUmQEBAQEBAJiYmCiMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyUmJiYmJiMoKCgoKCgoKCgoKCMjJSUjIyUlJiYmJSUlIyMjKCgoKCgoKCgjJSZAQEBAQEAmJSUmQEBAQEBAQCYlJUBAQEBAQEAmJiUKIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjJSYmJiYlKCgoKCgoKCgoKCgjIyUlIyMjJSUmJiYmJSMlJiUoKCgoKCgoIyMmQEBAQEBAQCYjJSZAQEBAQEBAJiUlQEBAQEBAQEAmJQojIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMlJiYmJiMoKCgoKCgoKCgoIyMlJSUmQEBAQEBAQCYlJSZAJSgoLy8oKCgjJSZAQEBAQEAmJSUmQEBAQEBAQEAmJSZAQEBAQEBAQCYlCiMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyUmJiYlKCgoKCgoKCgoIyMjJSUlJiZAQEAmJiYlJSMlJSYjKCgvLygoKCMlJSUlJSUlIyMjIyMlJSUlJSUlJSUjIyMlJSUlJSUlJSUKIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjJSYmJigoKCgoKCgoKCgjIyUlJSZAQEBAQEAmJiUlJkBAJiMoKC8vKCgoIyZAQEBAQEAmJSUmQEBAQEBAQEBAJiUlJkBAQEBAQEBAJgojIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMlJSYlKCgoKCgoKCgoIyMjJkBAQEBAQEBAQCYlJSZAQEBAJSMoKCgoKCMlJkBAQEBAQCYlJSZAQEBAQEBAQEAmJSZAQEBAQEBAQEAmCiMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMlJSgoKCgoKCgoKCMjIyVAQEBAQEBAQEAmJSUmQEBAQEAjKCMjIyMlJSZAQEBAQEBAJSUmQEBAQEBAQEBAQCYlJkBAQEBAQEBAQCYKIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMlJiYjKCgoKCgoKCgoKCMjJkBAQEBAQEBAJiUmQEBAQEBAJSgvLy8oKCMlJkBAQEBAQCYlJSZAQEBAQEBAQEAmJiUlJSUlJSUlJSUlJQojIyMjIyMjIyMjIyMjIyMjIyMjIyMjJSZAJSgoKCgoKCgoKCgoIyVAQEBAQEBAQCYlJkBAQEBAJigvKC8vLy8oIyVAQEBAQEBAJSUlQEBAQEBAQCYmJSUjJSUlJSUlJSUlJSUlCiMjIyMjIyMjIyMjIyMjIyMjIyUlJSZAQCYoLy8vKCgoKCgoKCMjJkBAQEBAQEAmJSZAQEBAQEAjLygvLy8vLyglJkBAQEBAQCYlJSUlJSUlJSUlJSUlJSUlJSYmQEBAQEBAQEAKIyMjIyMjIyMjIyMjIyMjIyMjJSZAQEAjKC8vLygoKCgoKCgoKCMmQEBAQEAmJiUmQEBAQEBAQC8vKC8vLy8oIyUmJiUlJSUlJSUlJSUlJSUlJSUlJiUlJSUmQEBAQEBAQEBAQAojIyMjJSUlJSUlJSUlJSUlJSZAQCYjKC8vLygoKCgoKCgoKCgoKCVAQEBAJiYlJkBAQEBAQEBALyovLyovKCMlJiYlJSUlJSUlJSYmQEBAQEBAQEBAQCYlJSZAQEBAQEBAQEBACiUlJSUlJSUlJSUlIyMlJSUmQEAmKCgoKCgoKCgoKCgoKCgoKCgoKCVAQCYmJSUlJSUlJSUlJSUqLy8qLy8oIyUmQCYmJiYmJiUmJkBAQEBAQEBAQEBAJiUmJkBAQEBAQEBAQEAKJSUlJSUlIyMjJSUjJSUmQEBAQCgoKCMoKCgoKCgoKCgoKCgoKCgoKCglJSUlJSUlJSUlJSUlJS8vLy8vIyMlJkBAQEBAQCYmJSZAQEBAQEBAQEBAQEAmJSZAQEBAQEBAQEBAQAolJSUlJSUlJSUlJSUmQEBAQEBAIygoIyMoKCMoKCgoKCgoKCgoKCgoKCgoIyUlJiYmJiYmJiUoKCgoLyglJSZAJiZAQEBAJiUmJkBAQEBAQEBAQEBAQCYlJkBAQEBAQEBAQEBACiUlJSUlJSUlJSUmQEBAQEBAQEBAJigoKCgoKCgoKCgoKCMoKCgoKCgoKCgoKCVAQEAmJSMjIygvLy8oIyYmJSYmJiZAQEAmJiZAQEBAQEBAQEBAQEAmJiUmQEBAQEBAQEBAQEAKJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSMoLy8jJSMoLygoIyUmJiUjKCgvLygvLy8vKCgoIyMoLy8vKCMlJiYmJiUlJkBAQCYmQEBAQEBAQEBAQEBAQCYmJSZAQEBAQEBAQEBAQAolJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSUoKCZAQEBAJS8vKCMlJSMjIyMjIygjIyMoKCMoKCgoKCgvKi8vKCUmQEBAQCYmJSYmQEBAQEBAQEBAQEBAJiYlJkBAQEBAQEBAQEBACiUlJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSgvKCMjIyMjIyMvLygoIyMjJSUlJSMjIyMoIyMjKC8vKCVAQEBAQEBAQCYmJSUlJSYmJiYmQCYmJiYmJiUlJSUlJSYmJiYmJiYmJSUKIyMlJSUlJSUlJSUlJSUlJSUjIyMjIyMjKCgjIyMjIyMjKCgoLyoqKioqKioqKioqKioqKi8vKCgjIyUlJSUmJiYmJiUlJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSUlJSUlJQojJSUlJSYmJiYmJSUlJSUlJSUlJSUlJSUjIyMlJSUmJiUlJSMlJSYmJkBAQEBAQCYmJSUlJSUlJiZAQEBAQEBAQEBAJiUlJSYmQEBAQEBAQEBAQEAmJiUlJiZAQEBAQEBAQEBACiZAQEAmJSUlJSUlJSUlJSUlJSUlJSUlJiZAQEBAQEAmJSUlJkBAQEBAQEBAQEBAQCYlJSUmJkBAQEBAQEBAQEBAQEAmJSUmQEBAQEBAQEBAQEBAQEAmJSUmQEBAQEBAQEBAQEAKIyMjIyMjIyMjIyMlJSUlJSUlJSZAQEBAQEBAQEAmJiUlJSZAQEBAQEBAQEBAQEAmJiUlJSZAQEBAQEBAQEBAQEBAJiYlJSZAQEBAQEBAQEBAQEBAJiYlJSZAQEBAQEBAQEBAQAo= */

namespace test
{
    class Program
    {

        static string UserName = "", DefUserName = "Abigail";
        static string GameName = "Xombies and Nambaz 2";

        static bool cheatmode = false;

        static string Buffer = "";
        const string thisisyouimg = 
        @"##%%%%%%&@&&&&&%*/*/***,////////////////////,,********/////((#%&%###############
%%%%%%%%&@&&&&&&&&%((((#/////////////////(((*,*******/////((#%%%################
%%%%%%%%%%%%%%%%(((((((((//////////(#####%%#%********/////((#%######%###########
*********#%%%%%%///////(((///(#(#((((((####%%(*******/////((%#######%#%#%#%#%###
*****,,,**%%%%%%#**,*((((#####(######(((###%%%*******/////(##/#%&%#(#%&%(///////
,,,,,,,,,,*(((/*,,/(((((#((##(#(##(#######%%%%%*****/////((#**//*********/%###%#
/*/****,,,*(%%%/(((((#(((((/#######%####(((((#%*****/////(#%,,,***,******%&(####
**,**/,,,,,*##((((#(((((////,####(//////((((((#/****////((##,,,,,,,,,,,*(%#,/###
***/**,,,,,,*(((((((((((/****,((((////////((((#%*****///((#,,,,,,,,,,,,*#((#*,(#
******,,,,,*/********//,,,,***(((/(////////((((#******//(#%*****/%%/(#////(##(,/
,,,,**,,,***,*********,,,,,,,***(((/////(((((((#%*****//(#/***,.,.,,,,,,,,,,,/(*
,,,,,,..,,,*,,*,*,,***,...,,,**,,((////////(((((%*****//(%****,.,**********.,(##
,,**,,. .,,*,,,,*,##*//,..,,,,,,*/((////////((((#****//(#%****,.,///////***.,&@@
**********,,,,,,,,((///*,.,,,,,,,,((//////////(((%***/((#%,***,.,//////****.,/##
,,,,,,,,,,,,,,,,,,//////*.,,,,.,,,,(///////////((%****/(#%****,.,////******,,/##
,,,,,,,,,,,,,,,,,,///////.,,,,,,,,,((///(((/(//((#***//##%****,.,*,,,,,,,,,,.*##
,,,,,,,,,,,,,,,,,,/(((((/..........((//((((((((((%**///(##......,//(((/((((*,*#%
,,,,,,,,,,,,,,,,,,(((((((,.*(((###(////((((((((#%***////(#@@&#/.*(((####%%&%*/&%
,,,,,,,,,,,,,,,,,,(#(((((/.*(((##(///(((((((((#%****///((#&@&%(,*&%%@@@%&@@@**&%
,,,,,,,*(,,##(..(##(#####(.,((##//////((((((##&*****///((#@@&#%,/&#&@@@%&@@@/,%#
%%%%%%###**###*.(########(.,(((///////((((##%##(#######%%%&&#&&,/%#@@@@#&@@@%,%%
#%%######//##(/.((((((((((,,/(//////(((((##%&#((#(#%#####%&&&&&,(%%%&%%%%%%%%#&&
#########/*%%#(,(#(#(#((((*,(////(((((((##%(%#######%%%%%%&&&&#,(%&@@@@%&@@@@%%%
%%%%%%%%%#(%&%&(%%%#######(((//((((((((##%#((%####%%%&%%&&&&@@@@%%&%%%%%%%%%##%%
(########((###%(%@@&&&&%%%#(((((((((((##%&%##&&(((((((##%%#%%%%##%%%%%%%%%%%###%
(((((((##(((#(#(#&&&&&%%%%((((((((((((#%&%%%%%%((/((((##&###%%%##%%%%#%%%&@@@@%%
(###(############%&&&&%&%%((((((((((##%#%%%%%%%(((((((#%&%%&&&&%#%&&&&%%#%%@@&%%
###################&@&&&&((((((((((##%%%%%%%%%%(((/(((#%&%%%%%%%&@@@@@&#&@@@@@&%
###################%&&&&&(((((((((##%##&@@@&%##(((((((#%@@@@@&%%@@@@@@&%&@@@@@&%
####################&&&&(((((((((##%###%&&&&%#&((((((##&@@@@@&#&@@@@@@&%&@@@@@@%
###################%&@&&((((((((##%%@@@@@@@&%&@((//((#%@@@@@@%%@@@@@@@&%&@@@@@&%
####################&&&((((((((##%###%%%%%###%&((//((#&&@@@&%#%&@@@@@@&%&@@@@@@&
####################%&#(((((((##%&@@@@@@&%%@@@@#(/(((%@@@@@@%%@@@@@@@@&%&@@@@@@@
####################%&(((((((###@@@@@@@&%%@@@@@((##%&&@@@@@&%&@@@@@@@@%%@@@@@@@@
###################&@(((((((((#%@@@@@@&%&@@@@@#(//(#%@@@@@@%%&@@@@@@@@%%%%%%%%%%
###############%#%@@#(((((((((#@@@@@@&%&@@@@&,(///(#&@@@@@&%%%%%%%%%%%%%%%%%&&&&
###############%@@@(///(((((((#@@@@@&%&@@@@@%((///(#&&&%%%%%%%%%%%%%%%%&@@@@@@@@
##%%%%%#%%%%%%&@@((*/((((((((((@@@@&%&@@@@@@%//*//#%@%%%%%%%&@@@@@@@@&%&@@@@@@@@
%%%%%%%%%%%%&@@@/(((((((((((((((&@&%%%%%%%%%#(///(%&@@@@@&%&@@@@@@@@@&%&@@@@@@@@
%%%%%%%%%%%&@@@@/(((((#(((((((((((%%%%%%%%%%(((/#%&%&@@@&%&&@@@@@@@@@&%&@@@@@@@@
%%%%%%%%%&@@@@@@@((((((##((/((((((((%@@@@%##/*//%&%&%&@@&&&@@@@@@@@@@&%&@@@@@@@@
%%%%%%%%%%%%%%%%%%%(*%%%/((#&&%(//(///*#*(#((/(#&%@&&@@@&&@@@@@@@@@@@&%&@@@@@@@@
%%%%%%%%%%%%%%%%%%%#*@@&%%#/(#######(#####/((#((%@@@@@@&%&@@@@@@@@@@&&%&@@@@@@@@
#%%%%%%%%%%%%%%#####,(###((((,/(((##/((((/,.*(#%%&&@@&%%%%%%%%%%%%%%%%%%%%%%%%%%
#%%%%%%&%%%%%%%%%%%%##%%%%%%%#%%&&@@@&&%%%%%%&&@@@@@@@&%%%&@@@@@@@@@&%%&&@@@@@@@
&@@@%##%%%%%%%%%%%%@@@@@@&%%&@@@@@@@@@@&%%&@@@@@@@@@@@&%&@@@@@@@@@@@&%&&@@@@@@@@
##########%%%%%@@@@@@@@@&%%&@@@@@@@@@@&%%&@@@@@@@@@@@&%%&@@@@@@@@@@@&%%&@@@@@@@@";

        static int MinRange = 1, MaxRange = 20, my = 0, maxHealth = 8;

        static int zombiecount = 0, health = maxHealth, guess = 0, ded = 0, playerPower = 0, score = 0;

        /*
            Deads:
            0: live
            1: dead by running out of guess and being hit on the head by a bar of iron
            2: dead by zombies
            3: dead y brick hitting the head
            other: unknown status
        */

        static int zombiePower = 0;

        static string[] effect = { "SWOSH", "POW", "PEW", "CLANG", "CLASH", "BANG", "BLORT", "BART", "FART", "SLAP", "SLOP", "SLAP-PA-PY GRAND-PA-PY" };
        static int effectL = effect.Length;

        static bool exitr = false;

        static Stopwatch stopWatch = new Stopwatch();

        static ConsoleColor currentForeground = Console.ForegroundColor;

        static ConsoleColor currentBackground = Console.BackgroundColor;

        static string xmlconfig = "./config.xan.xml", exe = "";

        static bool silent = false;

        static int helpme(string exe)
        {
            Console.WriteLine("-sl --SILENT\t\tMake the game silent");
            Console.WriteLine("-h, --HELP\t\tDisplay this message");
            return 0;
        }

        static int Main(string[] args)
        {

            exe = System.AppDomain.CurrentDomain.FriendlyName;

            if(args.Length < 1)
            {

            }
            else{
                for(int it = 0; it < args.Length; it++)
                {
                    string arg = args[it];

                    if(arg == "-h" || arg == "--HELP")
                    {
                        helpme(exe);
                        return 0;
                    }
                    else if(arg == "-sl" || arg == "--SILENT")
                    {
                        silent = true;
                    }
                }
            }

            if(silent == false)
            {
                Console.Beep();
            }

            stopWatch.Start();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);
            // Console.TreatControlCAsInput = true;

            bool quitt = false;

            // var log.logger = new Serilog.LoggerConfiguration();

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Welcome to " + GameName);
            if(!Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("Sorry, We don't serve people with 32 bit computers");
                return 0;
            }

            Console.WriteLine("To exit now or later, Type [exit] to exit the game.");
            Buffer = Console.ReadLine();
            if(Buffer.ToLower() == "exit")
            {
                Console.WriteLine("See Ya Latr");
                return 0;
            }

            Console.WriteLine("What is your name:");
            UserName = Console.ReadLine();
            if(UserName == "" || UserName == null)
            {
                UserName = DefUserName;
            }
            else if(UserName == DefUserName)
            {
                Console.WriteLine("So this is you");
                Console.WriteLine(thisisyouimg);
            }
            else if(UserName == "Andry Lie" || UserName == "andry lie" || UserName == "Andry lie" || UserName == "andry Lie")
            {
                cheatmode = true;
                Console.WriteLine("Who are you? Why are you looking like a powerful God, Are you cheating?");
            }

            Console.WriteLine("\nOh yeah, btw I am Mariah Kareyh, the female narrator that will guide you and also I know computers");

            Console.WriteLine("\n");
            Console.WriteLine("So, how many zombie do you want?");
            while(true)
            {
                Buffer = Console.ReadLine();
                if(Buffer.ToLower() == "exit")
                {
                    Console.WriteLine("We are exiting the game.");
                    break;
                }

                if(Int32.TryParse(Buffer, out zombiecount))
                {
                    Console.WriteLine("\nGood\n");
                    break;
                }
                else{
                    Console.WriteLine("This is not an int, 1 int = 1 zombie, 0.5 float = half a zombie, A string = ???");
                }
            }
            Console.WriteLine("You fight by guessing numbers to train yourself and increase your skill!\nIf you guess corectly and you killed the zombie, you win and get to live another day and fight again with another zombie!\nYou lose and you are DED, no more fighting.\nYou lose by running out of tries in the guessing game or losing to a zombie with a higher skill level.\nYou win by killinng the zombie and you can increase the change if winning by guessing correctly with as little mistake as possible.\nYour score and skill is multiplied depending on how much zombies you want to fight\n");
            Console.ReadLine();
            Console.WriteLine("Fight Fight Fight "+  UserName + "!\n\n");
            Thread.Sleep(2000);

            playerPower = RNGSys.RandomRandomNumber(zombiecount + 10, zombiecount + 20);
            
            while(true)
            {

                my = RNGSys.RandomRandomNumber(MinRange, MaxRange);

                if(ded != 0)
                {
                    break;
                }

                if(quitt == true)
                {
                    break;
                }

                health = maxHealth;
                while(true)
                {


                    if(health < 1 && cheatmode != true)
                    {
                        ded = 1;
                        break;
                    }

                    Console.WriteLine("My number is between " + MinRange + " and " + MaxRange);
                    Console.WriteLine("You have " + health + "guesses left");
                    Console.WriteLine("Your score is " + score);

                    Buffer = Console.ReadLine();
                    if(Buffer.ToLower() == "exit")
                    {
                        Console.WriteLine("We are exiting.");
                        quitt = true;
                        break;
                    }

                    if(Int32.TryParse(Buffer, out guess))
                    {
                        var dz = new SB();
                        dz.YesKareyh();
                        if(guess != my)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Nope");
                            Console.ForegroundColor = currentForeground;
                            if(guess < my)
                            {
                                Console.WriteLine("That is too smol of a bean");
                                score -= 1;
                            }
                            else if(guess > my){
                                Console.WriteLine("Sike, thats tru-, SYKE again, Too big");
                                score -= 2;
                            }
                            else{
                                Console.WriteLine("We don't know what is the problem, but we do know that it is not the right number");
                            }
                            Thread.Sleep(1000);
                            health -= 1;
                            playerPower -= RNGSys.RandomRandomNumber(1, 3);
                        }
                        else{
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("That is true.");
                            Console.ForegroundColor = currentForeground;
                            playerPower += RNGSys.RandomRandomNumber(1, 10);
                            score += 1;
                            break;
                        }
                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That is not integer number, floating point numbers are more difficult to play with");
                        Console.ForegroundColor = currentForeground;
                    }
                }

                if(ded != 0)
                {
                    break;
                }

                if(quitt == true)
                {
                    break;
                }

                zombiePower = RNGSys.RandomRandomNumber(zombiecount + 10 + (score / 2), zombiecount + 20 + (score / 2));

                if(zombiePower > (zombiecount + 15))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ya got a big zombie to deal with");
                    Console.ForegroundColor = currentForeground;
                }
                else{
                    Console.WriteLine("Thats no problem of you, he ain't strong");
                }

                Thread.Sleep(2000);

                int zhra = RNGSys.RandomRandomNumber(1, 45);

                var sz = new SB();
                sz.WaitZombie(effect, effectL);

                if(zhra == 1)
                {
                    Console.WriteLine("The zombie had a heart attack, snipping the point away from you");
                }
                else{

                    if(cheatmode != true && zombiePower > playerPower)
                    {
                        ded = 2;
                        Console.WriteLine("You are DED from the zombie");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You killed the zombie, let's see how much power ya got");

                    }
                }

                if(ded != 0)
                {
                    break;
                }

                if(quitt == true)
                {
                    break;
                }

                while(true)
                {
                    Console.WriteLine("You are now relaxing in your house. What do you want to do now?");
                    Console.WriteLine("Here is your stats: \n Your power is at " + playerPower + " \n and your score is at " + score);
                    Buffer = Console.ReadLine();

                    if(Buffer.ToLower() == "fight")
                    {
                        Console.WriteLine("You go back to fighting");
                        Thread.Sleep(1000);
                        break;
                    }
                    else if(Buffer.ToLower() == "quit")
                    {
                        Console.WriteLine("You decided to quit");
                        Thread.Sleep(1000);
                        quitt = true;
                        break;
                    }
                    else{
                        Console.WriteLine("?????");
                    }

                }


                if(ded != 0)
                {
                    break;
                }

                if(quitt == true)
                {
                    break;
                }


                int planed = RNGSys.RandomRandomNumber(1, 3);
                if(planed == 1)
                {
                    Console.WriteLine("At this moment, an airplane full of bricks happened fly by your path. \n\n");
                    Thread.Sleep(1000);
                    int bricked = RNGSys.RandomRandomNumber(1, 100);
                    if(bricked < 3 && cheatmode != true)
                    {   
                        Console.WriteLine("One of the bricks fall out and hit your hed, you are DED as a doornail.\n");
                        ded = 3;
                    }
                }

                if(ded != 0)
                {
                    break;
                }

                if(quitt == true)
                {
                    break;
                }
            }

            if(silent == false)
            {
                Console.Beep();
            }

            SaveData();

            // log.CloseAndFlush();
            if(silent == false)
            {
                Console.Beep();
            }

            Console.ForegroundColor = currentForeground;
            Console.BackgroundColor = currentBackground;

            return 0;
 
        }

        private static void ExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            /* do nothing */
        }

        private static string SaveData()
        {
            return "";
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine ("-------------------------------");
        }
    }

    class SB{
        Program p = new Program();

        public void WaitZombie(string[] effect, int effectL)
        {
            /* Kurukuru.Spinner k;
            k = new Kurukuru.Spinner("", () =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 5; i++) {
                    string eff = effect[RNGSys.RandomRandomNumber(0, effectL)];
                    // Kurukuru.Spinner.Text = "Fighting with " + eff + "...";
                    k.Text = "Fighting with " + eff + "...";
                    Thread.Sleep(1000);
                }
            }, Kurukuru.Patterns.Line);
            k.StartAsync(); */
            Thread th = new Thread(zpinnerz);
            for (int i = 0; i < 5; i++) {
                string eff = effect[RNGSys.RandomRandomNumber(0, effectL)];
                // Kurukuru.Spinner.Text = "Fighting with " + eff + "...";
                Console.WriteLine("Fighting with " + eff + "...");
                Thread.Sleep(1000);
            }
            th.Interrupt();
            Console.WriteLine("");
        }

        public void YesKareyh()
        {
            /*
            var k = new Kurukuru.Spinner();
            k.StartAsync("", () =>
            {
                // Kurukuru.Spinner.Text = "That is an Integer, but let's see if it is true";
                k.Text = "That is an Integer, but let's see if it is true";
                Thread.Sleep(2000);
            }, Kurukuru.Patterns.Line); */
            Thread th = new Thread(zpinnerz);
            Console.WriteLine("That is an Integer, but let's see if it is true");
            th.Start();
            Thread.Sleep(2000);
            th.Interrupt();
            Console.WriteLine("");

        }

        public void zpinnerz(object arg)
        {
            int speed = Convert.ToInt32(arg);
            try{
                while(true)
                {
                    var s = new CLUISpinner(@"/-\|");
                    Thread.Sleep(speed);
                    s.UpdateProgress();
                }
            }
            catch(ThreadInterruptedException e)
            {
                Console.WriteLine("e");
            }

        }
    }
}
