package mixter.infra.repositories

import mixter.domain.identity.UserId
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class InMemoryFollowerRepositorySpec extends AnyWordSpec with Matchers {
  "An InMemoryFollowerRepository" should {
    "have no followers for a userId when empty" in {
      //Given
      val repository = new InMemoryFollowerRepository()
      //When
      val followers = repository.getFollowers(AFollowee)
      //Then
      followers shouldBe empty
    }

    "return saved follower" in {
      //Given
      val repository = new InMemoryFollowerRepository()
      //When
      repository.saveFollower(AFollowee, AFollower)
      //Then
      val followers = repository.getFollowers(AFollowee)
      followers should contain only(AFollower)
    }

    "only return followers of followee" in {
      //Given
      val repository = new InMemoryFollowerRepository()
      repository.saveFollower(AFollowee, AFollower)
      repository.saveFollower(AnotherFollowee, AnotherFollower)

      //When
      val followers = repository.getFollowers(AFollowee)

      //Then
      followers should contain only(AFollower)
    }
    "remove follower of followee" in {
      //Given
      val repository = new InMemoryFollowerRepository()
      repository.saveFollower(AFollowee, AFollower)
      repository.saveFollower(AnotherFollowee, AFollower)

      //When
      repository.removeFollower(AFollowee, AFollower)

      //Then
      val followers = repository.getFollowers(AFollowee)
      followers should not contain AFollower
      val otherFollowers = repository.getFollowers(AnotherFollowee)
      otherFollowers should contain(AFollower)
    }
  }

  // followees
  private val AFollowee = UserId("followee@example.localhost")
  private val AnotherFollowee = UserId("anotherfollowee@example.localhost")

  // followers
  private val AFollower = UserId("follower@example.localhost")
  private val AnotherFollower = UserId("anotherfollower@example.localhost")
}
