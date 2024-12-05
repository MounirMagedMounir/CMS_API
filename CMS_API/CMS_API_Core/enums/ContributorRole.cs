using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.enums
{
    public enum ContributorRole
    {
        Writer,
        Publisher,
        CoAuthor,
        Translator,
        Editor,            // responseible for reviewing and editing articles
        Reviewer,          // Provides feedback and approval before publication
        FactChecker,       // Verifies facts and information in the articles
        Photographer,      // responseible for taking photographs to accompany articles
        GraphicDesigner,   // Creates graphics and visual content for articles
        Researcher,        // Conducts research to support article content
        SocialMediaManager,// Handles social media promotion and engagement
        ContentManager,    // Manages the overall content strategy and scheduling
        LayoutDesigner,    // Designs the layout and formatting of articles
        ContentCurator,    // Selects and organizes articles or content for publication
        DataAnalyst,      // Analyzes data related to article performance and audience engagement
    }


}
