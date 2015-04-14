package mixter.infra;

public class SpyEvenHandler<T> {
    boolean called = false;

    public void apply(T event) {
        called = true;
    }

    public boolean isCalled() {
        return called;
    }
}
