using System.Text.Json.Serialization;

namespace Carbon.Css
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CssBoxAlignment : byte
    {
        Unknown       = 0,
        Start         = 1, // flush left or top     
        End           = 2, // flush right or bottom  
        Center        = 3, // centered

        SelfStart     = 4, // self-start
        SelfEnd       = 5, // self-end

        // 
        // <baseline-position> 
        Baseline      = 7, // baseline
        FirstBaseline = 8, // first baseline
        LastBaseline  = 9, // last baseline

        // <content-distribution>
        SpaceAround   = 10, // space-around
        SpaceBetween  = 11, // space-between          
        SpaceEvenly   = 12, // space-evenly
        Stretch       = 13, // stretch
        
        // <overflow-position>? <content-position>
        SafeCenter    = 15,
        UnsafeCenter  = 16,
    }

    // <baseline-position> | <content-distribution> | <overflow-position>? <content-position>
    
    // <content-distribution> = space-between | space-around | space-evenly | stretch
    // <baseline-position> = [ first | last ]? baseline
    // <overflow-position> = unsafe | safe
    // <content-position> = center | start | end | flex-start | flex-end
}

// CSS Box Alignment v3
// https://drafts.csswg.org/css-align-3/#propdef-align-content

// Notes about flex-start

// https://stackoverflow.com/questions/50919447/flexbox-flex-start-self-start-and-start-whats-the-difference