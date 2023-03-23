﻿namespace TipsAndTricks.Services.Timing;

public class UtcTimeProvider : ITimeProvider {
    public DateTime Now => DateTime.UtcNow;

    public DateTime Today => DateTime.UtcNow.Date;
}