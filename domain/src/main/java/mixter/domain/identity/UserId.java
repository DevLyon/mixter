package mixter.domain.identity;

public class UserId {
    private String email;

    public UserId(String email) {
        if (email == null || email.trim().isEmpty()) {
            throw new UserEmailCannotBeEmpty();
        }
        this.email = email;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        UserId userId = (UserId) o;

        return !(email != null ? !email.equals(userId.email) : userId.email != null);

    }

    @Override
    public int hashCode() {
        return email != null ? email.hashCode() : 0;
    }

    @Override
    public String toString() {
        return email;
    }
}
