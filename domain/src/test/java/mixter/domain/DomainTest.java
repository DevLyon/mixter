package mixter.domain;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class DomainTest {
    protected List<Event> history(Event... events) {
        List<Event> eventHistory = new ArrayList<>();
        Collections.addAll(eventHistory, events);
        return eventHistory;
    }
}
