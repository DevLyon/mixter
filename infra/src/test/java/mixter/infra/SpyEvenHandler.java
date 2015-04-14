package mixter.infra;

public class SpyEvenHandler {
    boolean called = false;

    public void apply(EventA event) {
        called = true;
    }

    public boolean isCalled() {
        return called;
    }
}
