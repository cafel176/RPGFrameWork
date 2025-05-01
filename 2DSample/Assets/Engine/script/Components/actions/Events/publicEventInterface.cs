public interface publicEventInterface
{
    void resetNowList();
    bool canNowListRemove();
    void setCanDo(bool can);
    bool checkNowListEventConditions();
    void startEvent(string name, nowState n);
    void exit();
}
