// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Editor.Services;

public class AIService
{
    public const string Out_Of_Office = "打开项目（ Project2 ）";
    public const string LinkedIn_Well_Being = "添加实时报警表";
    public const string LinkedIn_Mental_Health = "添加 KPI 设备运行效率分析";

    private static Dictionary<string, Dictionary<string, string>> responses = new Dictionary<string, Dictionary<string, string>>
        {            
            {
                "Chinese",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Project2 项目已加载！以下为测试文本：\r\n\r\n主题: 不在办公室: [Your Name]\r\n\r\n您好 [收件人 姓名],\r\n\r\n我目前不在办公室，直到 [返回日期]，电子邮件访问权限有限。 如有紧急事务，请致电 [您的电话号码]。否则，我会在返回时回复您的电子邮件。\r\n\r\n谢谢你的理解。\r\n\r\n此致,\r\n[您的姓名]" },
                    { LinkedIn_Well_Being, "Embracing Work/Life Balance for Success\r\n\r\nHello LinkedIn network! Today, let's talk about a key to our well-being: work/life balance.\r\n\r\nIn our fast-paced world, balancing professional and personal life is not just a luxury, it's a necessity for health and productivity.\r\n\r\nKey Points\r\n\r\nEnhances Productivity: Less burnout, more motivation.\r\nBoosts Creativity: New experiences = fresh ideas.\r\nImproves Health: Mental and physical benefits.\r\nStrengthens Relationships: Quality time matters.\r\nLet's champion a culture where success and well-being go hand in hand. Share your work/life balance tips below!\r\n\r\n#WorkLifeBalance #Productivity #WellBeing"},
                    { LinkedIn_Mental_Health,"Embracing Work-Life Balance: Key to Well-Being \r\n\r\nIn today's fast-paced world, maintaining a work-life balance is crucial. It's not just beneficial – it's essential.\r\n\r\nWhy Important?\r\n\r\nBoosts Productivity: Rest leads to higher creativity and problem-solving.\r\nEnhances Well-being: Prevents burnout, promoting physical and mental health.\r\nImproves Relationships: Ensures time for family and friends.\r\nAchieving It:\r\n\r\nSet Boundaries: Define work hours to separate professional and personal life.\r\nPrioritize Tasks: Focus on what's urgent to manage time effectively.\r\nTake Breaks: Essential for mental refreshment and perspective.\r\nEmbrace Flexibility: Use flexible working options for a balanced day.\r\nLet's strive for balance to ensure sustained success and happiness.\r\n\r\n#WorkLifeBalance #Productivity #MentalHealth" },
                }
            },
            {
                "English",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Out of Office: [Your Name]\r\n\r\nDear [Recipient's Name],\r\n\r\nI am currently out of the office until [Return Date] and have limited email access. For urgent matters, please call me at [Your Phone Number]. Otherwise, I will respond to your email upon my return.\r\n\r\nThank you for your understanding.\r\n\r\nBest,\r\n[Your Name]" },
                    { LinkedIn_Well_Being, "Embracing Work/Life Balance for Success\r\n\r\nHello LinkedIn network! Today, let's talk about a key to our well-being: work/life balance.\r\n\r\nIn our fast-paced world, balancing professional and personal life is not just a luxury, it's a necessity for health and productivity.\r\n\r\nKey Points\r\n\r\nEnhances Productivity: Less burnout, more motivation.\r\nBoosts Creativity: New experiences = fresh ideas.\r\nImproves Health: Mental and physical benefits.\r\nStrengthens Relationships: Quality time matters.\r\nLet's champion a culture where success and well-being go hand in hand. Share your work/life balance tips below!\r\n\r\n#WorkLifeBalance #Productivity #WellBeing"},
                    { LinkedIn_Mental_Health,"Embracing Work-Life Balance: Key to Well-Being \r\n\r\nIn today's fast-paced world, maintaining a work-life balance is crucial. It's not just beneficial – it's essential.\r\n\r\nWhy Important?\r\n\r\nBoosts Productivity: Rest leads to higher creativity and problem-solving.\r\nEnhances Well-being: Prevents burnout, promoting physical and mental health.\r\nImproves Relationships: Ensures time for family and friends.\r\nAchieving It:\r\n\r\nSet Boundaries: Define work hours to separate professional and personal life.\r\nPrioritize Tasks: Focus on what's urgent to manage time effectively.\r\nTake Breaks: Essential for mental refreshment and perspective.\r\nEmbrace Flexibility: Use flexible working options for a balanced day.\r\nLet's strive for balance to ensure sustained success and happiness.\r\n\r\n#WorkLifeBalance #Productivity #MentalHealth" },
                }
            },
            {
                "Casual",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Stepping Out for a Bit - [Your Name]\r\n\r\nHey [Recipient's Name],\r\n\r\nJust a quick heads up – I'm out of the office until [Return Date] and might not be checking emails much. If something urgent pops up, just give me a ring at [Your Phone Number]. Otherwise, I'll catch up on emails when I'm back.\r\n\r\nThanks for understanding!\r\n\r\nCheers,\r\n[Your Name]"},
                    { LinkedIn_Well_Being, "Let's Talk Work-Life Balance\r\n\r\nHey LinkedIn fam! Quick chat: how's your work-life balance doing?\r\n\r\nHere's the deal: balancing work and play isn't just nice, it's necessary!\r\n\r\nWhy, you ask?\r\n\r\nWork Smarter, Not Harder: Too much work = burnout. Taking breaks = getting more done.\r\nHello, Creativity: Ever get great ideas while chilling? That's what I'm talking about!\r\nStay Healthy: All work and no play makes us stressed and tired. Who wants that?\r\nFamily and Friends Time: Remember them? Yeah, they're important.\r\nSo, let's make work-life balance cool again! What's your secret to keeping things chill? Drop a comment!\r\n\r\n#RealTalk #WorkLifeChill #BalanceIsKey #StayCool" },
                    { LinkedIn_Mental_Health, "Hey everyone,\r\n\r\nJust wanted to drop a quick note about something I've been thinking a lot about lately: work-life balance.\r\n\r\nLet's face it, we're often caught up in the hustle, but have you noticed how taking a step back actually makes you more productive? Yep, it's true! A little break can spark some serious creativity.\r\n\r\nHere's the deal:\r\n\r\nWork Smart: It's not about working non-stop, it's about working efficiently.\r\nHealth is Wealth: Avoid burnout, folks! Your mind and body will thank you.\r\nFamily & Friends Time: Because life's too short to miss out on fun moments.\r\nTips to keep it balanced:\r\n\r\nDraw the Line: Work time is work time, chill time is chill time.\r\nWhat's Important?: Not everything is urgent. Prioritize!\r\nBreaks are Bae: Short walks, coffee breaks, vacations - they all count.\r\nFlexi is Sexy: Flexi-hours or work-from-home can be a game-changer.\r\nSo, let's keep it balanced and rock our work and personal lives!\r\n\r\n#WorkLifeBalance #StayProductive #StayHealthy" },
                }
            },
            {
                "Neutral",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Out of Office: [Your Name]\r\n\r\nDear [Recipient's Name],\r\n\r\nI am out of the office until [Return Date] with limited email access. For urgent matters, please call [Your Phone Number]. I will respond to your email upon my return.\r\n\r\nRegards,\r\n[Your Name]" },
                    { LinkedIn_Well_Being, "Understanding the Importance of Work/Life Balance\r\n\r\nIn the professional realm, it's important to acknowledge the significance of work/life balance. This concept isn't just about dividing time between work and personal life; it's about achieving an equilibrium that supports both professional responsibilities and personal well-being.\r\n\r\nKey Points:\r\n\r\nProductivity: A balanced approach often leads to sustained productivity levels.\r\nCreativity: Diverse experiences outside of work can enhance problem-solving and innovation.\r\nHealth: Managing work stress is crucial for maintaining both mental and physical health.\r\nRelationships: Allocating time for personal relationships is essential for a well-rounded life.\r\nCreating a work environment that values balance can contribute to both employee satisfaction and organizational success.\r\n\r\nThoughts on maintaining work/life balance are welcome in the comments.\r\n\r\n#WorkLifeBalance #ProfessionalDevelopment #EmployeeWellbeing" },
                    { LinkedIn_Mental_Health, "Maintaining Work-Life Balance: Essential for Sustainable Success\r\n\r\nIn the current professional landscape, achieving a work-life balance is increasingly recognized as a key factor for long-term success and well-being.\r\n\r\nWhy Work-Life Balance Matters?\r\n\r\nEnhances Productivity: Regular breaks and downtime can lead to improved focus and efficiency.\r\nSupports Health: Balancing work and personal life helps prevent burnout and promotes overall well-being.\r\nStrengthens Personal Relationships: Allocating time outside work nurtures important personal connections.\r\nStrategies for Achieving Balance:\r\n\r\nSet Clear Boundaries: Establish specific work hours to ensure personal time remains undisturbed.\r\nPrioritize Effectively: Distinguish between urgent and less critical tasks to manage workload efficiently.\r\nRegular Breaks: Incorporating short breaks and vacation time is vital for mental and physical rejuvenation.\r\nFlexible Working Arrangements: Where possible, utilize flexibility in work schedules to accommodate personal needs.\r\nWork-life balance is a continuous, individual process and is crucial for maintaining long-term professional and personal health.\r\n\r\n#WorkLifeBalance #ProfessionalDevelopment #HealthAndWellness" }
                }
            },
            {
                "Formal",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Temporary Absence from Office: [Your Name]\r\n\r\nDear [Recipient's Name],\r\n\r\nI trust this message finds you in good health. Please be advised that I am currently out of the office and will not return until [Return Date]. During this period, my access to email will be significantly restricted.\r\n\r\nIn cases of urgent matters, I kindly request that you contact me directly via telephone at [Your Phone Number]. For all other inquiries, I assure you they will be addressed promptly upon my return.\r\n\r\nI appreciate your understanding and apologize for any inconvenience this may cause.\r\n\r\nYours sincerely,\r\n\r\n[Your Name]\r\n[Your Position]\r\n[Your Company]\r\n[Contact Information]" },
                    { LinkedIn_Well_Being, "The Imperative of Work/Life Balance in Professional Excellence\r\n\r\nGreetings, Esteemed LinkedIn Colleagues,\r\n\r\nIn our rapidly evolving professional landscape, I wish to highlight an increasingly crucial aspect: the work/life balance. This equilibrium is not merely a concept but a fundamental practice that significantly impacts our productivity, creativity, health, and interpersonal relationships.\r\n\r\nKey Points to Consider:\r\n\r\nEnhanced Productivity: Studies reveal that well-balanced professionals demonstrate higher efficiency and motivation.\r\nStimulation of Creativity: Diverse experiences outside of work foster innovative thinking and problem-solving skills.\r\nHealth and Well-being: A balanced approach to work and life is instrumental in reducing stress-related ailments, thereby promoting both mental and physical health.\r\nRobust Relationships: Allocating time for personal relationships is critical for emotional support and overall life satisfaction.\r\nIt is incumbent upon us as leaders and professionals to advocate for and implement policies that support work/life balance. This practice is not only beneficial for individual employees but is also conducive to the long-term success of our organizations.\r\n\r\nI invite you to share your insights and strategies on achieving an optimal work/life balance.\r\n\r\nBest Regards,\r\n\r\n[Your Name]\r\n\r\n#WorkLifeBalance #ProfessionalDevelopment #Leadership #WellBeing" },
                    { LinkedIn_Mental_Health, "The Imperative of Work-Life Balance for Professional Success and Personal Well-being\r\n\r\nIn the realm of modern professional endeavors, the significance of maintaining an equilibrium between work and personal life cannot be overstated. This balance is not merely a luxury but an essential component for long-term success and well-being.\r\n\r\nKey Reasons for Emphasizing Work-Life Balance:\r\n\r\nEnhanced Productivity: Adequate rest and detachment from work-related tasks catalyze creativity and effective problem-solving capabilities.\r\nImproved Health and Well-being: A well-balanced lifestyle is instrumental in preventing occupational burnout, thereby safeguarding both mental and physical health.\r\nStrengthened Personal Relationships: Allocating time for personal relationships is crucial for emotional support and overall life satisfaction.\r\nStrategies for Cultivating Balance:\r\n\r\nImplementing Clear Boundaries: Establishing definitive work hours is essential to distinguish professional responsibilities from personal time.\r\nEffective Prioritization of Tasks: Identifying and addressing urgent tasks efficiently is key to managing workload and reducing stress.\r\nIncorporating Regular Breaks: Breaks are fundamental for mental rejuvenation, offering new perspectives, particularly in challenging situations.\r\nUtilizing Flexible Work Arrangements: Flexibility in work schedules can significantly contribute to a harmonious daily routine, accommodating both career and personal commitments.\r\nIn conclusion, work-life balance is a pivotal factor in achieving sustained professional success and personal fulfillment. It requires conscious and continual efforts to integrate effectively into our daily lives.\r\n\r\n#ProfessionalDevelopment #WorkLifeBalance #EmployeeWellbeing #CareerSuccess" },
                }
            },
            {
                "Simplify",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Out of Office: [Your Name]\r\n\r\nI'm out of the office until [Return Date]. For urgent issues, call [Your Phone Number]. Will respond to emails on my return.\r\n\r\nThanks,\r\n[Your Name]" },
                    { LinkedIn_Well_Being, "Prioritize Work/Life Balance\r\n\r\nHey LinkedIn! Let's talk about balancing work and life.\r\n\r\nKey Points:\r\n\r\nMore Balance = More Productivity\r\nBreaks Boost Creativity\r\nGood for Health and Relationships\r\nLet's create a culture where productivity and well-being go hand in hand.\r\n\r\nYour strategies for balance? Comment below!\r\n\r\n#WorkLifeBalance #Productivity #WellBeing" },
                    { LinkedIn_Mental_Health, "Prioritize Work-Life Balance\r\n\r\nBalancing work and life is key to success and well-being.\r\n\r\nBenefits:\r\n\r\nBoosts Productivity: Rest enhances creativity.\r\nImproves Well-being: Prevents burnout.\r\nStrengthens Relationships: Ensures time for loved ones.\r\n\r\nTips:\r\n\r\nSet Boundaries: Keep work and personal life separate.\r\nPrioritize & Take Breaks: Manage time well, recharge often.\r\nFlexibility: Adapt work schedules for balance.\r\nBalance leads to happiness and success!\r\n\r\n#WorkLifeBalance #Productivity #WellBeing" },
                }
            },
            {

                "Extend",
                new Dictionary<string, string>
                {
                    { Out_Of_Office, "Subject: Out of Office: [Your Name]\r\n\r\nDear [Recipient's Name],\r\n\r\nThank you for reaching out. Please note that I am currently out of the office and will not be available until [Return Date]. During this period, my access to email will be limited.\r\n\r\nFor urgent matters, feel free to contact me directly at [Your Phone Number]. I will do my best to respond promptly. For all other inquiries or immediate assistance, please contact [Colleague's Name] at [Colleague's Email Address], who will be available to provide support in my absence.\r\n\r\nI appreciate your understanding and patience during this time. Rest assured, I will address all emails and pending matters as soon as I return to the office.\r\n\r\nWarm regards,\r\n\r\n[Your Name]\r\n[Your Position]\r\n[Your Company]\r\n[Your Contact Information]" },
                    { LinkedIn_Well_Being, "Navigating the Tightrope of Work/Life Balance\r\n\r\nGreetings, LinkedIn community! In the midst of our bustling professional lives, there's a silent chord that often goes unnoticed: the delicate balance between work and personal life. Today, let's explore why nurturing this balance is not just beneficial, but essential.\r\n\r\nThe digital age has dissolved traditional boundaries, making it challenging to separate 'office hours' from 'personal time'. But here's why finding that equilibrium is crucial:\r\n\r\nEnhances Productivity: It’s a myth that more hours equal more productivity. Studies show that well-rested, balanced individuals are more efficient and driven. Reducing burnout isn’t just good for employees; it’s smart business.\r\n\r\nFuels Creativity and Innovation: Ever noticed how your best ideas often come during downtime? A mind at ease is a breeding ground for creativity and innovation. Diverse experiences and restful breaks can lead to breakthroughs in problem-solving and creativity.\r\n\r\nImproves Physical and Mental Health: Chronic stress, a byproduct of overwork, can lead to serious health issues. Balancing work and life is crucial for mental clarity, emotional well-being, and physical health. It's about working smarter, not harder.\r\n\r\nStrengthens Personal Relationships: Our relationships define us. Spending quality time with family and friends rejuvenates our spirits and gives us a sense of belonging and support. Remember, no one on their deathbed ever wished they'd spent more time at the office.\r\n\r\nEnhances Job Satisfaction: Employees who feel they have a good balance between their work and personal lives often feel more satisfied with their jobs and less likely to leave. This reduces turnover and builds a more loyal and content workforce.\r\n\r\nAs professionals, entrepreneurs, or leaders, it's crucial to model and encourage a culture that values this balance. Let's cultivate environments where productivity thrives alongside personal well-being.\r\n\r\nI'd love to hear from you! How do you maintain a healthy work/life balance? What strategies have you found effective? Let's share and learn from each other. Your insights are valuable! Comment below!\r\n\r\n#WorkLifeBalance #MentalHealth #Leadership #Productivity #HealthyLiving" },
                    { LinkedIn_Mental_Health, "Prioritize Work-Life Balance\r\n\r\nBalancing work and life is key to success and well-being.\r\n\r\nBenefits:\r\n\r\nBoosts Productivity: Rest enhances creativity.\r\nImproves Well-being: Prevents burnout.\r\nStrengthens Relationships: Ensures time for loved ones.\r\n\r\nTips:\r\n\r\nSet Boundaries: Keep work and personal life separate.\r\nPrioritize & Take Breaks: Manage time well, recharge often.\r\nFlexibility: Adapt work schedules for balance.\r\nBalance leads to happiness and success!\r\n\r\n#WorkLifeBalance #Productivity #WellBeing" },
                }
            }
        };

    public static bool IsValidInputText(string inputText)
    {
        if (inputText == Out_Of_Office ||
            inputText == LinkedIn_Well_Being ||
            inputText == LinkedIn_Mental_Health)
        {
            return true;
        }

        return false;
    }

    public static string GetResponse(string commandName, string key)
    {
        if (key == null || !responses[commandName].ContainsKey(key))
        {
            return "如需实际处理提示，请将组件连接至优选的人工智能服务。您可使用提示视图中“提示建议”部分的某一项来获取示例结果。";
        }

        return responses[commandName][key];
    }
}
